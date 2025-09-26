
using Autofac;

namespace Odering.Infrastructure;

public static class CompositionRoot
{
    private static IContainer _container;


    internal static void SetContainer(IContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container), "Container cannot be null");
    }

    internal static ILifetimeScope BeginLifeTimeScope()
    {
        if (_container is null)
        {
            throw new InvalidOperationException("Please rebuild the solution");
        }
        return _container.BeginLifetimeScope();
    }
}