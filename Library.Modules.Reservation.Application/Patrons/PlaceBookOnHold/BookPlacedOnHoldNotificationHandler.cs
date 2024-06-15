using Library.BuildingBlocks.EventBus;
using Library.Modules.Reservation.IntegrationEvents;
using MediatR;

namespace Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

public class BookPlacedOnHoldNotificationHandler(IEventBus eventBus)
    : INotificationHandler<BookPlacedOnHoldNotification>
{
    public async Task Handle(BookPlacedOnHoldNotification notification, CancellationToken cancellationToken)
    {
        await eventBus.Publish(new BookPlacedOnHoldIntegrationEvent
        {
            Id = notification.Id,
            BookId = notification.DomainEvent.BookId.Value,
            LibraryBranchId = notification.DomainEvent.LibraryBranchId.Value,
            PatronId = notification.DomainEvent.PatronId.Value,
            Till = notification.DomainEvent.Till,
            OccurredOn = notification.DomainEvent.OccurredOn
        });
    }
}