using Odering.Infrastructure.Caching;
using Ordering.Domain.ForeignExchange;

namespace Odering.Infrastructure.Domain.ForeignExchanges;

internal class ConversionRatesCacheKey : ICacheKey<ConversionRatesCache>
{
    public string CacheKey => "ConversionRatesCache";
}