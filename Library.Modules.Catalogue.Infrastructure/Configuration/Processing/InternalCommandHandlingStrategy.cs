using Autofac;
using Library.Modules.Catalogue.Application.Contracts;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Processing;

public class InternalCommandHandlingStrategy
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public InternalCommandHandlingStrategy(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }
    
    private IAsyncDocumentSession OpenAsyncSession() => _documentStoreHolder.OpenAsyncSession();

    public async Task Handle(InternalCommandBase command)
    {
        using (var session = OpenAsyncSession())
        {
            var handlerType = typeof(InternalCommandHandler<>).MakeGenericType(command.GetType());
            
            var scope = CatalogueCompositionRoot.BeginLifetimeScope();
            var commandHandler = scope.Resolve(handlerType) as InternalCommandHandler;
            
            try
            {
                await session.StoreAsync(command, command.Id);

                await SetCommandProcessing(session, command);

                await commandHandler.Handle(command);

                await SetCommandProcessed(session, command);
            }
            catch (Exception e)
            {
                await HandleError(session, command, e);
            }
        }
    }
    
    private static async Task SetCommandProcessing(IAsyncDocumentSession session, InternalCommandBase message)
    {
        message.Status = InternalCommandStatus.Processing;
        await session.SaveChangesAsync();
    }

    private static async Task SetCommandProcessed(IAsyncDocumentSession session, InternalCommandBase message)
    {
        message.Status = InternalCommandStatus.Processed;
        message.ProcessedAt = DateTime.UtcNow;
        await session.SaveChangesAsync();
    }

    private static async Task HandleError(IAsyncDocumentSession session, InternalCommandBase message, Exception exception)
    {
        message.Status = InternalCommandStatus.Error;
        message.ProcessingError = exception.ToString();
        message.LastFailedAt = DateTime.UtcNow;
        await session.SaveChangesAsync();
    }
}