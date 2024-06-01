using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons.Events;
using MediatR;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class BookHoldCanceledByPatronDomainEventHandler(IHoldRepository holdRepository)
    : INotificationHandler<BookHoldCanceledByPatronDomainEvent>
{
    public async Task Handle(BookHoldCanceledByPatronDomainEvent @event, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(@event.HoldId);
        hold.ApplyPatronCancelDecision();
    }
}