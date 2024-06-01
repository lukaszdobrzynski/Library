using Library.BuildingBlocks.Application.Events;
using Library.Modules.Reservation.Domain.Patrons.Events;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class BookHoldCanceledByPatronNotification : IDomainEventNotification<BookHoldCanceledByPatronDomainEvent>
{
    public Guid Id { get; set; }
    public BookHoldCanceledByPatronDomainEvent DomainEvent { get; }

    public BookHoldCanceledByPatronNotification(BookHoldCanceledByPatronDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}