using Autofac;

namespace Library.Modules.Catalogue.Infrastructure.Configuration;

internal class CatalogueCompositionRoot
{
    private static IContainer _container;

    public static void SetContainer(IContainer container)
    {
        _container = container;
    }

    public static ILifetimeScope BeginLifetimeScope()
    {
        return _container.BeginLifetimeScope();
    }
}