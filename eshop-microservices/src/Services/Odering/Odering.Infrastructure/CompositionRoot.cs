
using Autofac;

namespace Odering.Infrastructure;

public class CompositionRoot
{
    private static IContainer _container;


    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }
}