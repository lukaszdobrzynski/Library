using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.Mediation;
using Library.Modules.Catalogue.Infrastructure.Configuration.Processing;
using Serilog;

namespace Library.Modules.Catalogue.Infrastructure.Configuration;

public static class CatalogueStartup
{
    private static IContainer _container;

    public static void Init(RavenDatabaseSettings ravenSettings, ILogger logger, IEventBus eventBus)
    {
        var catalogueModuleLogger =  logger.ForContext("Module", "Catalogue");
        
        ConfigureContainer(ravenSettings, eventBus);
        
        SubscriptionsStartup.Initialize(catalogueModuleLogger);
        EventBusStartup.Initialize();
    }

    private static void ConfigureContainer(RavenDatabaseSettings ravenSettings, IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new DataAccessModule(ravenSettings));
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new MediationModule());
        containerBuilder.RegisterModule(new ProcessingModule());

        _container = containerBuilder.Build();
        
        CatalogueCompositionRoot.SetContainer(_container);
    }
}