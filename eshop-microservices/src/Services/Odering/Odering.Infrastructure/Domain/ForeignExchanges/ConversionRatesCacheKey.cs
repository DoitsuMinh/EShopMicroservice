using Odering.Infrastructure.Caching;

namespace Odering.Infrastructure.Domain.ForeignExchanges;

internal class ConversionRatesCacheKey : ICacheKey<ConversionRatesCache>
{
    public string CacheKey => "ConversionRatesCache";
}