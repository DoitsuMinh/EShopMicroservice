using Newtonsoft.Json;
using Odering.Infrastructure.Caching;
using Ordering.Domain.ForeignExchange;
using Ordering.Domain.ForeignExchange.ExternalService;

namespace Odering.Infrastructure.Domain.ForeignExchanges;

public class ForeignExchange : IForeignExchange
{
    private readonly ICacheStore _cacheStore;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ForeignExchange(ICacheStore cacheStore, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _cacheStore = cacheStore;
        _httpClient = httpClientFactory.CreateClient("FreeCurrencyApi");
        _config = config;
        _httpClient.BaseAddress = new Uri(_config["FreeCurrencyApi:BaseUrl"].ToString());
    }

    public async Task<List<ConversionRate>> GetConversionRatesAsync()
    {
        try
        {
            var ratesCache = _cacheStore.Get(new ConversionRatesCacheKey());

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
    }

    private async Task<List<ConversionRate>> GetConversionRatesFromExternalApiAsync()
    {
        var request = new FreeCurrencyApiRequest(
            _config["FreeCurrencyApi:ApiKey"] ?? string.Empty,
            _config["FreeCurrencyApi:BaseCurrency"] ?? string.Empty,
            _config["FreeCurrencyApi:SupportedCurrencies"]?.Split(',').ToList() ?? new List<string>()
            );

        var response = await GetLastesRatesAsync(request);

        var rates = new List<ConversionRate>();

        if (response.Data is not null)
        {
            foreach (var rate in response.Data)
            {
                // Forward conversion (Base currency to target currency)
                rates.Add(new ConversionRate(request.BaseCurrency, rate.Key, rate.Value));

                // Reverse conversion (Target currency to base currency)
                if (rate.Value != 0)
                {
                    rates.Add(new ConversionRate(rate.Key, request.BaseCurrency, 1 / rate.Value));
                }
                else
                {
                    throw new InvalidOperationException($"Zero conversion rate detected for currency {rate.Key}.");
                }
            }
        }
        return rates;
    }

    private async Task<FreeCurrencyApiResponse> GetLastesRatesAsync(FreeCurrencyApiRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var currencies = string.Join(",", request.Currencies);

            var url = request.URL;

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
