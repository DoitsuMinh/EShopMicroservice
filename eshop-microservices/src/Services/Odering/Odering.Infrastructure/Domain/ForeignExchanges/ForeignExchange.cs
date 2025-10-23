using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Odering.Infrastructure.Caching;
using Ordering.Domain.ForeignExchange;
using Ordering.Domain.ForeignExchange.ExternalService;

namespace Odering.Infrastructure.Domain.ForeignExchanges;

public class ForeignExchange : IForeignExchange, IDisposable
{
    private readonly ICacheStore _cacheStore;
    private readonly HttpClient _httpClient;
    private readonly FreeCurrencyApiSettings _settings;

    // Semaphore to ensure thread-safe access to the cache
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public ForeignExchange(ICacheStore cacheStore, IHttpClientFactory httpClientFactory, IOptions<FreeCurrencyApiSettings> options)
    {
        _cacheStore = cacheStore;
        _httpClient = httpClientFactory.CreateClient("FreeCurrencyApi");
        _settings = options.Value;
    }

    public async Task<List<ConversionRate>> GetConversionRatesAsync()
    {
        // try cache without locking
        var ratesCache = _cacheStore.Get(new ConversionRatesCacheKey());
        if (ratesCache != null)
        {
            return ratesCache.Rates;
        }

        // Only lock API call and cache update
        await _semaphore.WaitAsync();
        try
        {
            // double check
            ratesCache = _cacheStore.Get(new ConversionRatesCacheKey());
            if (ratesCache != null)
            {
                return ratesCache.Rates;
            }

            var rates = await GetConversionRatesFromExternalApiAsync();            

            _cacheStore.Add(
                new ConversionRatesCache(rates), 
                new ConversionRatesCacheKey(), 
                DateTime.Now.Date.AddDays(1));

            return rates;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve conversion rates.", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }

    private async Task<List<ConversionRate>> GetConversionRatesFromExternalApiAsync()
    {
        var url = $"latest?apikey={_settings.ApiKey}&base_currency={_settings.BaseCurrency}&currencies={string.Join(",", _settings.SupportedCurrencies)}";

        var response = await GetLastesRatesAsync(url);

        var rates = new List<ConversionRate>();

        if (response.Data is not null)
        {
            foreach (var rate in response.Data)
            {
                // Forward conversion (Base currency to target currency)
                rates.Add(new ConversionRate(_settings.BaseCurrency, rate.Key, rate.Value));

                // Reverse conversion (Target currency to base currency)
                if (rate.Value != 0)
                {
                    rates.Add(new ConversionRate(rate.Key, _settings.BaseCurrency, 1 / rate.Value));
                }
                else
                {
                    throw new InvalidOperationException($"Zero conversion rate detected for currency {rate.Key}.");
                }
            }
        }
        return rates;
    }

    private async Task<FreeCurrencyApiResponse> GetLastesRatesAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);

            var result = JsonConvert.DeserializeObject<FreeCurrencyApiResponse>(jsonContent);

            return result ?? throw new JsonException();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to retrieve currency rates from external API.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new InvalidOperationException("Request timeout while retrieving currency rates from external API.", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Invalid response format from external API.", ex);
        }
    }

    // Communication with external API. Here is only mock.
    private List<ConversionRate> GetConversionRatesFromMock()
    {
        return [
            new ConversionRate("USD", "AUD", 1.52m),
            new ConversionRate("AUD", "USD", 0.66m),
            new ConversionRate("USD", "EUR", 0.85m),
            new ConversionRate("EUR", "USD", 1.18m),
        ];
    }
}
