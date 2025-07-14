using Odering.Infrastructure.Caching;
using System.Collections;
using System.Collections.Specialized;

namespace Ordering.IntegrationTests.SeedWork;

public class CachesStore : ICacheStore
{
    private ListDictionary dictionary = new ListDictionary();
    public void Add<T>(T item, ICacheKey<T> key, TimeSpan? expiration = null)
    {
        dictionary.Add(key, item);
    }

    public void Add<T>(T item, ICacheKey<T> key, DateTime? absoluteExpiration = null)
    {
        dictionary.Add(key, item);
    }

    public T Get<T>(ICacheKey<T> key) where T : class
    {
        return dictionary[key] as T ?? throw new InvalidOperationException("The key does not exist in the cache or the value is null.");
    }

    public void Remove<T>(ICacheKey<T> key)
    {
        dictionary.Remove(key);
    }
}