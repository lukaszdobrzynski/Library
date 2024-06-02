using Library.BuildingBlocks.Application.Events;
using Library.Modules.Reservation.Domain.Patrons.Events;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class BookHoldCanceledByPatronNotification : IDomainEventNotification<HoldCanceledDomainEvent>
{
    public Guid Id { get; set; }
    public HoldCanceledDomainEvent DomainEvent { get; }

    public BookHoldCanceledByPatronNotification(HoldCanceledDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}