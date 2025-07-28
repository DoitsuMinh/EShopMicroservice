
using Autofac;

namespace Odering.Infrastructure;

public class CompositionRoot
{
    private static IContainer _container;


    internal static void SetContainer(IContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container), "Container cannot be null");
    }

    internal static ILifetimeScope BeginLifeTimeScope()
    {
        return _container.BeginLifetimeScope();
    }
}