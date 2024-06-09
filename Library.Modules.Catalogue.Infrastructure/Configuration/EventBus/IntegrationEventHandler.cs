using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Catalogue.Infrastructure.Inbox;
using Newtonsoft.Json;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;

public class IntegrationEventHandler<T> : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public async Task Handle(T integrationEvent)
    {
        var scope = CatalogueCompositionRoot.BeginLifetimeScope();
        var documentStoreHolder = scope.Resolve<IDocumentStoreHolder>();

        using (var session = documentStoreHolder.OpenAsyncSession())
        {
            var type = integrationEvent.GetType().FullName;
            var data = JsonConvert.SerializeObject(integrationEvent);

            var inboxMessage = new InboxMessage
            {
                Type = type,
                Data = data
            };

            await session.StoreAsync(inboxMessage);
            await session.SaveChangesAsync();
        }
    }
}