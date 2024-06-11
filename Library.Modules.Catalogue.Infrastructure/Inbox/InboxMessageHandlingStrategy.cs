using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using MediatR;
using Newtonsoft.Json;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Inbox;

public class InboxMessageHandlingStrategy
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly IMediator _mediator;

    public InboxMessageHandlingStrategy(IDocumentStoreHolder documentStoreHolder, IMediator mediator)
    {
        _documentStoreHolder = documentStoreHolder;
        _mediator = mediator;
    }

    private IAsyncDocumentSession OpenAsyncSession() => _documentStoreHolder.OpenAsyncSession();
    
    public async Task Handle(InboxMessage message)
    {
        using (var session = OpenAsyncSession())
        {
            try
            {
                await session.StoreAsync(message, message.Id);
            
                await SetMessageProcessing(session, message);
                
                var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .Single(x => message.Type.Contains(x.GetName().Name));

                var messageType = messageAssembly.GetType(message.Type);
                var notification = JsonConvert.DeserializeObject(message.Data, messageType);

                await _mediator.Publish((INotification) notification);

                await SetMessageProcessed(session, message);
            }
            catch (Exception e)
            {
                await HandleError(session, message, e);
            }
        }
    }
    
    private static async Task SetMessageProcessing(IAsyncDocumentSession session, InboxMessage message)
    {
        message.Status = InboxMessageProcessingStatus.Processing;
        await session.SaveChangesAsync();
    }

    private static async Task SetMessageProcessed(IAsyncDocumentSession session, InboxMessage message)
    {
        message.Status = InboxMessageProcessingStatus.Processed;
        await session.SaveChangesAsync();
    }

    private static async Task HandleError(IAsyncDocumentSession session, InboxMessage message, Exception exception)
    {
        message.Status = InboxMessageProcessingStatus.Error;
        message.ProcessingError = exception.ToString();
        message.LastFailedAt = DateTime.UtcNow;
        await session.SaveChangesAsync();
    }
}