using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Patrons.Events;

namespace Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

public class BookPlacedOnHoldNotification : IDomainNotification<BookPlacedOnHoldDomainEvent>
{
    public BookPlacedOnHoldDomainEvent DomainEvent { get; }
    public Guid Id { get; }
    
    public BookPlacedOnHoldNotification(BookPlacedOnHoldDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}