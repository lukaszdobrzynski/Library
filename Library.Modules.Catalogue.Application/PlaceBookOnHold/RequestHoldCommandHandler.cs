using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Models;
using Newtonsoft.Json;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class RequestHoldCommandHandler : InternalCommandHandler<RequestHoldCommand>
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly IDomainNotificationsRegistry _domainNotificationsRegistry;

    public RequestHoldCommandHandler(IDocumentStoreHolder documentStoreHolder, 
        IDomainNotificationsRegistry domainNotificationsRegistry)
    {
        _documentStoreHolder = documentStoreHolder;
        _domainNotificationsRegistry = domainNotificationsRegistry;
    }
    
    protected override async Task HandleConcreteCommand(RequestHoldCommand command)
    {
        using (var session = _documentStoreHolder.OpenAsyncSessionWithOptimisticConcurrencyControl())
        {
            var book = await session.LoadAsync<Book>(command.BookId.ToString());

            if (book.Status != BookStatus.Available)
            {
                var bookHoldRejectedNotification = new BookHoldRejectedNotification
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.Parse(book.Id),
                    LibraryBranchId = book.LibraryBranchId,
                    PatronId = command.PatronId,
                    OccurredOn = DateTime.UtcNow,
                    ExternalHoldRequestId = command.ExternalHoldRequestId
                };
                
                var bookHoldRejectedNotificationType = _domainNotificationsRegistry.GetName(bookHoldRejectedNotification.GetType());
                var bookHoldRejectedNotificationData = JsonConvert.SerializeObject(bookHoldRejectedNotification);
                var bookHoldRejectedOutboxMessage = OutboxMessage.CreateSubmitted(Guid.NewGuid().ToString(), 
                    DateTime.UtcNow, 
                    bookHoldRejectedNotificationType, 
                    bookHoldRejectedNotificationData);;

                await StoreAndSaveChanges(session, bookHoldRejectedOutboxMessage);
                return;
            }

            book.Status = BookStatus.OnHold;

            var bookHoldGrantedNotification = new BookHoldGrantedNotification
            {
                Id = Guid.NewGuid(),
                BookId = Guid.Parse(book.Id),
                LibraryBranchId = book.LibraryBranchId,
                PatronId = command.PatronId,
                OccurredOn = DateTime.UtcNow,
                ExternalHoldRequestId = command.ExternalHoldRequestId
            };

            var bookHoldGrantedNotificationType = _domainNotificationsRegistry.GetName(bookHoldGrantedNotification.GetType());
            var bookHoldGrantedNotificationData = JsonConvert.SerializeObject(bookHoldGrantedNotification);
            var bookHoldGrantedOutboxMessage = OutboxMessage.CreateSubmitted(Guid.NewGuid().ToString(), 
                DateTime.UtcNow, 
                bookHoldGrantedNotificationType, 
                bookHoldGrantedNotificationData);
            await StoreAndSaveChanges(session, bookHoldGrantedOutboxMessage);
        }
    }
    
    private static async Task StoreAndSaveChanges(IAsyncDocumentSession session, OutboxMessage outboxMessage)
    {
        await session.StoreAsync(outboxMessage);
        await session.SaveChangesAsync();
    }
}