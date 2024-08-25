using Autofac;
using Library.BuildingBlocks.Application;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.Logging;
using Library.Modules.Catalogue.Infrastructure.Configuration.Mediation;
using Library.Modules.Catalogue.Infrastructure.Configuration.Processing;
using Serilog;

namespace Library.Modules.Catalogue.Infrastructure.Configuration;

public static class CatalogueStartup
{
    private static IContainer _container;

    public static void Init(IDocumentStoreHolder documentStoreHolder, IExecutionContextAccessor executionContextAccessor, ILogger logger, IEventBus eventBus)
    {
        var catalogueModuleLogger =  logger.ForContext("Module", "Catalogue");
        
        ConfigureContainer(documentStoreHolder, executionContextAccessor, catalogueModuleLogger, eventBus);
        
        SubscriptionsStartup.Initialize(catalogueModuleLogger);
        EventBusStartup.Initialize();
    }

    private static void ConfigureContainer(IDocumentStoreHolder documentStoreHolder, IExecutionContextAccessor executionContextAccessor, ILogger logger, IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new LoggingModule(logger));
        containerBuilder.RegisterModule(new DataAccessModule(documentStoreHolder));
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new MediationModule());
        containerBuilder.RegisterModule(new ProcessingModule());
        
        containerBuilder.RegisterInstance(executionContextAccessor);

        _container = containerBuilder.Build();
        
        CatalogueCompositionRoot.SetContainer(_container);
    }
}