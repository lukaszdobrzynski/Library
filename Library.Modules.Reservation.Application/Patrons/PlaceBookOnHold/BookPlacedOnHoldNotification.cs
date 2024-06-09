using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Patrons.Events;

namespace Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

public class BookPlacedOnHoldNotification : IDomainEventNotification<BookPlacedOnHoldDomainEvent>
{
    public BookPlacedOnHoldDomainEvent DomainEvent { get; }
    public Guid Id { get; set; }
    
    public BookPlacedOnHoldNotification(BookPlacedOnHoldDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}