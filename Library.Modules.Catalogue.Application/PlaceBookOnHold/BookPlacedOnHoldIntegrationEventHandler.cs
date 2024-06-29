using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Reservation.IntegrationEvents;
using MediatR;

namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class BookPlacedOnHoldIntegrationEventHandler : INotificationHandler<BookPlacedOnHoldIntegrationEvent>
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public BookPlacedOnHoldIntegrationEventHandler(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }
    
    public async Task Handle(BookPlacedOnHoldIntegrationEvent notification, CancellationToken cancellationToken)
    {
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            var command = RequestHoldCommand.CreateSubmitted(
                notification.BookId,
                notification.LibraryBranchId,
                notification.PatronId,
                notification.Till,
                notification.HoldRequestId);

            await session.StoreAsync(command, cancellationToken);
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}