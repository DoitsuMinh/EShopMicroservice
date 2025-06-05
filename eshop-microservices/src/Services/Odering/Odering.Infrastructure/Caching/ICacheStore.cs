namespace Odering.Infrastructure.Caching;

public interface ICacheStore
{
    /// <summary>
    /// Adds an item to the cache with a specified key and optional expiration time.
    /// </summary>
    void Add<T>(T item, ICacheKey<T> key, TimeSpan? expiration = null);

    /// <summary>
    /// Adds an item to the cache with a specified key and optional absolute expiration time.
    /// </summary>
    void Add<T>(T item, ICacheKey<T> key, DateTime? absoluteExpiration = null);

    /// <summary>
    /// Retrieves an item from the cache using the specified key.
    /// </summary>
    T Get<T>(ICacheKey<T> key) where T : class;

    /// <summary>
    /// Removes an item from the cache using the specified key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    void Remove<T>(ICacheKey<T> key);
}