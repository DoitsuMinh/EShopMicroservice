using Microsoft.Extensions.Caching.Memory;
using Odering.Infrastructure.Caching;

namespace Ordering.Infrastructure.Caching;

public class MemoryCacheStore : ICacheStore
{
    private readonly IMemoryCache _memoryCache;
    private readonly Dictionary<string, TimeSpan> _expirationConfiguration;

    public MemoryCacheStore(IMemoryCache memoryCache, Dictionary<string, TimeSpan> expirationConfiguration)
    {
        _memoryCache = memoryCache;
        _expirationConfiguration = expirationConfiguration;
    }

    public void Add<T>(T item, ICacheKey<T> key, TimeSpan? expiration = null)
    {
        var cachedObjectName = item?.GetType().Name ?? throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        TimeSpan timeSpan;
        if (expiration.HasValue)
        {
            timeSpan = expiration.Value;
        }
        else
        {
            timeSpan = _expirationConfiguration[cachedObjectName];
        }

        _memoryCache.Set(key.CacheKey, item, timeSpan);
    }

    public void Add<T>(T item, ICacheKey<T> key, DateTime? absoluteExpiration = null)
    {
        DateTimeOffset offset;
        if(absoluteExpiration.HasValue)
        {
            offset = absoluteExpiration.Value;
        }
        else
        {
            offset = DateTimeOffset.MaxValue;
        }

        _memoryCache.Set(key.CacheKey, item, offset);
    }

    public T Get<T>(ICacheKey<T> key) where T : class
    {
        if(_memoryCache.TryGetValue(key.CacheKey, out T cachedItem))
        {
            return cachedItem;
        }

        return null;
    }

    public void Remove<T>(ICacheKey<T> key)
    {
        _memoryCache.Remove(key.CacheKey);
    }
}