namespace Odering.Infrastructure.Caching;

public interface ICacheKey<T>
{
    string CacheKey { get; }
}