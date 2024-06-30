using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Models;
using Newtonsoft.Json;

namespace Library.Modules.Catalogue.Application.RestoreBookAvailability;

public class MarkBookAvailableForReservationCommandHandler : InternalCommandHandler<MarkBookAvailableForReservationCommand>
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly IDomainNotificationsRegistry _domainNotificationsRegistry;
    
    public MarkBookAvailableForReservationCommandHandler(IDocumentStoreHolder documentStoreHolder, IDomainNotificationsRegistry domainNotificationsRegistry)
    {
        _documentStoreHolder = documentStoreHolder;
        _domainNotificationsRegistry = domainNotificationsRegistry;
    }
    
    protected override async Task HandleConcreteCommand(MarkBookAvailableForReservationCommand command)
    {
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            var book = await session.LoadAsync<Book>(command.BookId.ToString());
            book.Status = BookStatus.Available;
            book.DueDate = null;

            var notification = new BookMadeAvailableForReservationNotification
            {
                Id = Guid.NewGuid(),
                BookId = Guid.Parse(book.Id),
                LibraryBranchId = book.LibraryBranchId
            };

            var notificationType = _domainNotificationsRegistry.GetName(notification.GetType());
            var notificationData = JsonConvert.SerializeObject(notification);

            var outboxMessage = OutboxMessage.CreateSubmitted(Guid.NewGuid().ToString(), 
                DateTime.UtcNow,
                notificationType, 
                notificationData);

            await session.StoreAsync(outboxMessage);
            await session.SaveChangesAsync();
        }
    }
}