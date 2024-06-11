﻿using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Catalogue.Infrastructure.Inbox;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

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
            if (await IsMessageAlreadyStored(session, integrationEvent.Id))
                return;
            
            var type = integrationEvent.GetType().FullName;
            var data = JsonConvert.SerializeObject(integrationEvent);

            var inboxMessage = InboxMessage.CreateSubmitted(integrationEvent.Id, integrationEvent.OccurredOn, type, data);
            
            await session.StoreAsync(inboxMessage);
            await session.SaveChangesAsync();
        }
    }

    private static Task<bool> IsMessageAlreadyStored(IAsyncDocumentSession session, Guid notificationId)
    {
         return session.Query<InboxMessage>()
            .Customize(x => x.WaitForNonStaleResults())
            .AnyAsync(x => x.IntegrationEventId == notificationId);
    }
        
}