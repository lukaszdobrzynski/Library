using Library.Modules.Catalogue.Application;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Models;
using MediatR;
using Newtonsoft.Json;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Outbox;

public class OutboxMessageHandlingStrategy
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly IMediator _mediator;
    private readonly IDomainNotificationsRegistry _domainNotificationsRegistry;

    public OutboxMessageHandlingStrategy(IDocumentStoreHolder documentStoreHolder, 
        IMediator mediator, IDomainNotificationsRegistry domainNotificationsRegistry)
    {
        _domainNotificationsRegistry = domainNotificationsRegistry;
        _mediator = mediator;
        _documentStoreHolder = documentStoreHolder;
    }
    
    public async Task Handle(OutboxMessage message)
    {
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            try
            {
                await session.StoreAsync(message, message.Id);
            
                await SetMessageProcessing(session, message);

                var type = _domainNotificationsRegistry.GetType(message.Type);
                var notification = JsonConvert.DeserializeObject(message.Data, type);
            
                await _mediator.Publish((INotification) notification);

                await SetMessageProcessed(session, message);
            }
            catch (Exception e)
            {
                await HandleError(session, message, e);
            }
        }
    }
    
    private static async Task SetMessageProcessing(IAsyncDocumentSession session, OutboxMessage message)
    {
        message.Status = OutboxMessageProcessingStatus.Processing;
        await session.SaveChangesAsync();
    }

    private static async Task SetMessageProcessed(IAsyncDocumentSession session, OutboxMessage message)
    {
        message.Status = OutboxMessageProcessingStatus.Processed;
        message.ProcessedAt = DateTime.UtcNow;
        await session.SaveChangesAsync();
    }

    private static async Task HandleError(IAsyncDocumentSession session, OutboxMessage message, Exception exception)
    {
        message.Status = OutboxMessageProcessingStatus.Error;
        message.ProcessingError = exception.ToString();
        message.LastFailedAt = DateTime.UtcNow;
        await session.SaveChangesAsync();
    }
}