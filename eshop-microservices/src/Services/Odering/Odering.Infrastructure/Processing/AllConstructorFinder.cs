using Autofac.Core.Activators.Reflection;
using System.Collections.Concurrent;
using System.Reflection;

namespace Odering.Infrastructure.Processing;

internal class AllConstructorFinder : IConstructorFinder
{
    private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> _cache = new();
    public ConstructorInfo[] FindConstructors(Type targetType)
    {
        var result = _cache.GetOrAdd(targetType,t => t.GetTypeInfo().DeclaredConstructors.ToArray());

        return result.Length > 0 ? result :
            // throw new NoConstructorsFoundException
            throw new NoConstructorsFoundException(targetType, this);
    }
}