using Autofac;
using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;

public class EventBusStartup
{
    public static void Initialize()
    {
        var scope = CatalogueCompositionRoot.BeginLifetimeScope();
        var eventBus = scope.Resolve<IEventBus>();
        
        //eventBus.Subscribe();
    }
}