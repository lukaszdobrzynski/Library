using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Application.RestoreBookAvailability;
using Library.Modules.Reservation.IntegrationEvents;
using MediatR;

namespace Library.Modules.Catalogue.Application.CancelHold;

public class HoldCancelledIntegrationEventHandler : INotificationHandler<HoldCancelledIntegrationEvent>
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public HoldCancelledIntegrationEventHandler(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }
    
    public async Task Handle(HoldCancelledIntegrationEvent notificationEvent, CancellationToken cancellationToken)
    {
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            var command = MarkBookAvailableForReservationCommand.CreateSubmitted(
                notificationEvent.BookId, notificationEvent.LibraryBranchId, notificationEvent.PatronId);

            await session.StoreAsync(command, cancellationToken);
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}