using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;

namespace Library.Modules.Catalogue.Infrastructure.Configuration;

public static class CatalogueStartup
{
    private static IContainer _container;

    public static void Init(IEventBus eventBus)
    {
        ConfigureContainer(eventBus);
    }

    private static void ConfigureContainer(IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new EventBusModule(eventBus));

        _container = containerBuilder.Build();
        
        CatalogueCompositionRoot.SetContainer(_container);
    }
}