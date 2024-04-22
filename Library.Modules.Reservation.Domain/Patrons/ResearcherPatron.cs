using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons.Events;
using Library.Modules.Reservation.Domain.Patrons.Rules;

namespace Library.Modules.Reservation.Domain.Patrons;

public class ResearcherPatron : Entity, IAggregateRoot
{
    public PatronId Id { get; private set; }
    
    private ResearcherPatron(PatronId id)
    {
        Id = id;
        AddDomainEvent(new ResearcherPatronCreatedDomainEvent(Id));
    }

    public static ResearcherPatron Create(Guid id)
    {
        return new ResearcherPatron(new PatronId(id));
    }

    public void PlaceOnHold(BookToHold bookToHold, List<ActiveHold> activeHolds, List<OverdueCheckout> overdueCheckouts)
    {
        CheckRule(new PatronCannotPlaceHoldOnAlreadyRegisteredHoldRule(activeHolds, bookToHold.BookId));
        CheckRule(new PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule(overdueCheckouts));

        AddDomainEvent(new BookPlacedOnHoldDomainEvent(bookToHold.BookId, Id, bookToHold.LibraryBranchId));
    }
    
    public void CancelHold(BookOnHold bookOnHold, List<ActiveHold> activeHolds)
    {
        CheckRule(new PatronCannotCancelHoldOwnedByAnotherPatronRule(Id, bookOnHold));
        CheckRule(new PatronCannotCancelUnregisteredHoldRule(bookOnHold, activeHolds));

        AddDomainEvent(new BookHoldCanceledDomainEvent(bookOnHold.BookId, bookOnHold.PatronId, bookOnHold.LibraryBranchId));
    }
}