using Library.BuildingBlocks.EventBus;
using Library.Modules.Reservation.IntegrationEvents;
using MediatR;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class CancelHoldDecisionAppliedNotificationHandler(IEventBus eventBus) : INotificationHandler<CancelHoldDecisionAppliedNotification>
{
    public async Task Handle(CancelHoldDecisionAppliedNotification notification, CancellationToken cancellationToken)
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