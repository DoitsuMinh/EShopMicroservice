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
        var ratesCache = _cacheStore.Get(new ConversionRatesCacheKey());
        if (ratesCache != null)
        {
            return ratesCache.Rates;
        }

        List<ConversionRate> rates = GetConversionRatesFromExternalApi();

        _cacheStore.Add(new ConversionRatesCache(rates), new ConversionRatesCacheKey(), TimeSpan.FromDays(1));
        return rates;
    }

    // This should be in DB
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
