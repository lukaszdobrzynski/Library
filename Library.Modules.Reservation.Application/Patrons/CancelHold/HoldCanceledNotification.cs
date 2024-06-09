using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Patrons.Events;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class HoldCanceledNotification : IDomainEventNotification<HoldCanceledDomainEvent>
{
    public Guid Id { get; set; }
    public HoldCanceledDomainEvent DomainEvent { get; }

    public HoldCanceledNotification(HoldCanceledDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}