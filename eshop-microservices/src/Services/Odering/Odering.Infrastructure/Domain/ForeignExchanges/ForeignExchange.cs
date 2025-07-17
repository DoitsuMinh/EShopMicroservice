using Odering.Infrastructure.Caching;
using Odering.Infrastructure.ExternalServices.FreeCurrencyApi;
using Odering.Infrastructure.ExternalServices.FreeCurrencyApi.Models;
using Ordering.Domain.ForeignExchange;
using Microsoft.Extensions.Options;
using Serilog;

namespace Odering.Infrastructure.Domain.ForeignExchanges;

public class ForeignExchange : IForeignExchange
{
    private readonly ICacheStore _cacheStore;
    private readonly IFreeCurrencyApiService _freeCurrencyApiService;
    private readonly FreeCurrencyApiOptions _options;
    private readonly ILogger _logger;

    public ForeignExchange(
        ICacheStore cacheStore,
        IFreeCurrencyApiService freeCurrencyApiService,
        IOptions<FreeCurrencyApiOptions> options,
        ILogger logger)
    {
        _cacheStore = cacheStore;
        _freeCurrencyApiService = freeCurrencyApiService;
        _options = options.Value;
        _logger = logger;
    }

    public List<ConversionRate> GetConversionRates()
    {
        try
        {
            _logger.Information("Attempting to retrieve conversion rates from cache");
            
            var cacheKey = new ConversionRatesCacheKey();
            var ratesCache = _cacheStore.Get(cacheKey);
            if (ratesCache != null)
            {
                _logger.Information("Conversion rates retrieved from cache successfully. Count: {RatesCount}", ratesCache.Rates.Count);
                return ratesCache.Rates;
            }

            _logger.Information("No cached conversion rates found. Fetching from external API");
            
            var rates = GetConversionRatesFromExternalApiAsync().GetAwaiter().GetResult();

            _logger.Information("Successfully retrieved {RatesCount} conversion rates from external API. Caching for 1 hour", rates.Count);
            
            _cacheStore.Add(new ConversionRatesCache(rates), new ConversionRatesCacheKey(), TimeSpan.FromHours(1));
            return rates;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve conversion rates from external API. Falling back to hardcoded rates");

            // Fallback to hardcoded rates if external API fails
            return GetFallbackRates();
        }
    }

    private async Task<List<ConversionRate>> GetConversionRatesFromExternalApiAsync()
    {
        var request = new FreeCurrencyApiRequest
        {
            ApiKey = _options.ApiKey,
            BaseCurrency = _options.BaseCurrency,
            Currencies = _options.SupportedCurrencies
        };

        _logger.Information("Calling external API for conversion rates with base currency {BaseCurrency} and {CurrencyCount} target currencies", 
            _options.BaseCurrency, _options.SupportedCurrencies.Count);

        var response = await _freeCurrencyApiService.GetLatestRatesAsync(request);
        var rates = new List<ConversionRate>();

        _logger.Debug("Processing {ResponseCount} currency rates from API response", response.Data.Count);

        // Convert API response to ConversionRate objects
        foreach (var rate in response.Data)
        {
            // Forward conversion (USD to target currency)
            rates.Add(new ConversionRate(_options.BaseCurrency, rate.Key, rate.Value));

            // Reverse conversion (target currency to USD)
            if (rate.Value != 0)
            {
                rates.Add(new ConversionRate(rate.Key, _options.BaseCurrency, 1 / rate.Value));
            }
            else
            {
                _logger.Warning("Zero conversion rate detected for currency {Currency}. Skipping reverse conversion", rate.Key);
            }
        }

        // Add cross-currency conversions if needed
        var initialRateCount = rates.Count;
        AddCrossCurrencyRates(rates);
        
        _logger.Information("Added {CrossCurrencyCount} cross-currency conversion rates", rates.Count - initialRateCount);

        return rates;
    }

    private void AddCrossCurrencyRates(List<ConversionRate> rates)
    {
        _logger.Debug("Adding cross-currency conversion rates");
        
        // Example: Add EUR to AUD conversion using USD as base
        var eurToUsd = rates.FirstOrDefault(r => r.SourceCurrency == "EUR" && r.TargetCurrency == "USD");
        var usdToAud = rates.FirstOrDefault(r => r.SourceCurrency == "USD" && r.TargetCurrency == "AUD");

        if (eurToUsd != null && usdToAud != null)
        {
            var eurToAudRate = eurToUsd.Factor * usdToAud.Factor;
            rates.Add(new ConversionRate("EUR", "AUD", eurToAudRate));
            rates.Add(new ConversionRate("AUD", "EUR", 1 / eurToAudRate));
            
            _logger.Debug("Added EUR-AUD cross-currency rates: EUR->AUD={EurToAudRate:F4}, AUD->EUR={AudToEurRate:F4}", 
                eurToAudRate, 1 / eurToAudRate);
        }
        else
        {
            _logger.Warning("Unable to create EUR-AUD cross-currency rates. EUR->USD: {EurToUsdExists}, USD->AUD: {UsdToAudExists}", 
                eurToUsd != null, usdToAud != null);
        }
    }

    private List<ConversionRate> GetFallbackRates()
    {
        _logger.Warning("Using fallback currency rates due to external API failure");
        
        var fallbackRates = new List<ConversionRate>
        {
            new ConversionRate("USD", "AUD", 1.52m),
            new ConversionRate("AUD", "USD", 0.66m),
            new ConversionRate("USD", "EUR", 0.85m),
            new ConversionRate("EUR", "USD", 1.18m),
        };
        
        _logger.Information("Fallback rates loaded: {FallbackRateCount} conversion rates available", fallbackRates.Count);
        
        return fallbackRates;
    }
}
