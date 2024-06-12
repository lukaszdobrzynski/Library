using Library.BuildingBlocks.EventBus;
using Library.Modules.Reservation.IntegrationEvents;
using MediatR;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class HoldCancelledNotificationHandler(IEventBus eventBus) : INotificationHandler<HoldCanceledNotification>
{
    public async Task Handle(HoldCanceledNotification notification, CancellationToken cancellationToken)
    {
        await eventBus.Publish(new HoldCancelledIntegrationEvent
        {
            Id = notification.Id,
            OccurredOn = notification.DomainEvent.OccurredOn,
            HoldId = notification.DomainEvent.HoldId.Value,
            LibraryBranchId = notification.DomainEvent.LibraryBranchId.Value,
            PatronId = notification.DomainEvent.PatronId.Value,
            BookId = notification.DomainEvent.BookId.Value
        });
    }
}