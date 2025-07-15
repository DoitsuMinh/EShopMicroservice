using Odering.Infrastructure.Caching;
using Ordering.Domain.ForeignExchange;

namespace Odering.Infrastructure.Domain.ForeignExchanges;

public class ForeignExchange : IForeignExchange
{
    private readonly ICacheStore _cacheStore;

    public ForeignExchange(ICacheStore cacheStore)
    {
        _cacheStore = cacheStore;
    }

    public List<ConversionRate> GetConversionRates()
    {
        try
        {
            var cacheKey = new ConversionRatesCacheKey();
            var ratesCache = _cacheStore.Get(cacheKey);
            if (ratesCache != null)
            {
                return ratesCache.Rates;
            }

            List<ConversionRate> rates = GetConversionRatesFromExternalApi();

            _cacheStore.Add(new ConversionRatesCache(rates), new ConversionRatesCacheKey(), TimeSpan.FromDays(1));
            return rates;
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            throw new InvalidOperationException("Failed to retrieve conversion rates.", ex);

        }
    }

    // This should be real-time data from an external API or in DB.
    private List<ConversionRate> GetConversionRatesFromExternalApi()
    {
        return new List<ConversionRate>
        {
            //new ConversionRate("USD", "EUR", 0.85m),
            //new ConversionRate("EUR", "USD", 1.18m),
            new ConversionRate("USD", "AUD", 1.52m),
            new ConversionRate("AUD", "USD", 0.66m),
        };
    }
}
