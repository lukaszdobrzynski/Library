using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;

namespace Library.Modules.Catalogue.Infrastructure.Configuration;

public static class CatalogueStartup
{
    private static IContainer _container;

    public static void Init(RavenDatabaseSettings ravenSettings, IEventBus eventBus)
    {
        ConfigureContainer(ravenSettings, eventBus);
        
        EventBusStartup.Initialize();
    }

    private static void ConfigureContainer(RavenDatabaseSettings ravenSettings, IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new DataAccessModule(ravenSettings));
        containerBuilder.RegisterModule(new EventBusModule(eventBus));

        _container = containerBuilder.Build();
        
        CatalogueCompositionRoot.SetContainer(_container);
    }
}