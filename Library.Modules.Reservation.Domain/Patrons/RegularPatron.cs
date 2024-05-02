using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons.Events;
using Library.Modules.Reservation.Domain.Patrons.Rules;

namespace Library.Modules.Reservation.Domain.Patrons;

public class RegularPatron : Entity, IAggregateRoot
{
    public PatronId Id { get; private set; }
    
    private RegularPatron(PatronId id)
    {
        Id = id;
        AddDomainEvent(new RegularPatronCreatedDomainEvent(Id));
    }

    public static RegularPatron Create(Guid id)
    {
        return new RegularPatron(new PatronId(id));
    }

    public void PlaceOnHold(BookToHold bookToHold, List<ActiveHold> activeHolds, List<OverdueCheckout> overdueCheckouts)
    {
        CheckRule(new PatronCannotPlaceHoldOnExistingHoldRule(activeHolds, bookToHold.BookId));
        CheckRule(new RegularPatronCannotPlaceHoldOnRestrictedBookRule(bookToHold.BookCategory));
        CheckRule(new RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(activeHolds));
        CheckRule(new PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule(overdueCheckouts));

        AddDomainEvent(new BookPlacedOnHoldDomainEvent(bookToHold.BookId, Id, bookToHold.LibraryBranchId));
    }

    public void CancelHold(BookOnHold bookOnHold, List<ActiveHold> activeHolds)
    {
        CheckRule(new PatronCannotCancelHoldOwnedByAnotherPatronRule(Id, bookOnHold));
        CheckRule(new PatronCannotCancelNonExistingHoldRule(bookOnHold, activeHolds));

        AddDomainEvent(new BookHoldCanceledDomainEvent(bookOnHold.BookId, bookOnHold.PatronId, bookOnHold.LibraryBranchId));
    }
}