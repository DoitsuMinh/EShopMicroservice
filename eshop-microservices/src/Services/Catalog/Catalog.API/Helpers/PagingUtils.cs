namespace Catalog.API.Helpers;

public static class PagingUtils
{
    public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        return source.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static IQueryable<T> Page<T>(this IQueryable<T> source, int page, int pageSize)
    {
        return source.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
