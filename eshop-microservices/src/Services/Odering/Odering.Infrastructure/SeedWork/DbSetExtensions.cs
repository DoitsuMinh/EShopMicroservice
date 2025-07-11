using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Odering.Infrastructure.SeedWork;

public static class DbSetExtensions
{
    public static IQueryable<T> IncludePaths<T>(this IQueryable<T> source, params string[] navigationPaths) where T : class
    {
        if(!(source.Provider is EntityQueryProvider))
        {
            return source;
        }

        return source.Include(string.Join(".", navigationPaths));
    }
}
