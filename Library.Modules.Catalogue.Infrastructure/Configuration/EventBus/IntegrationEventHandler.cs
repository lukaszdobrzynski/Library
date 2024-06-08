using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;

public class IntegrationEventHandler<T> : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public Task Handle(T integrationEvent)
    {
        var scope = CatalogueCompositionRoot.BeginLifetimeScope();
        var documentStoreHolder = scope.Resolve<IDocumentStoreHolder>();

        return Task.CompletedTask;
    }
}