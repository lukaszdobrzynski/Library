using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons.Events;
using MediatR;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class HoldCanceledDomainEventHandler(IHoldRepository holdRepository)
    : INotificationHandler<HoldCanceledDomainEvent>
{
    public async Task Handle(HoldCanceledDomainEvent @event, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(@event.HoldId);
        hold.ApplyCancelDecision();
    }
}