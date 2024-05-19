using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons.Events;
using Library.Modules.Reservation.Domain.Patrons.Rules;

namespace Library.Modules.Reservation.Domain.Patrons;

public class Patron : Entity, IAggregateRoot
{
    public PatronId Id { get; }

    private PatronType _patronType;

    private Patron()
    {
        // for EF only
    }
    
    private Patron(PatronId id, PatronType patronType)
    {
        Id = id;
        _patronType = patronType;
        
        AddDomainEvent(new PatronCreatedDomainEvent(Id));
    }

    public static Patron CreateRegular(Guid id)
    {
        return new Patron(new PatronId(id), PatronType.Regular);
    }
    
    public static Patron CreateResearcher(Guid id)
    {
        return new Patron(new PatronId(id), PatronType.Researcher);
    }

    public void PlaceOnHold(BookToHold bookToHold, List<ActiveHold> activeHolds, List<OverdueCheckout> overdueCheckouts)
    {
        CheckRule(new PatronCannotPlaceHoldOnExistingHoldRule(activeHolds, bookToHold.BookId));
        CheckRule(new RegularPatronCannotPlaceHoldOnRestrictedBookRule(_patronType, bookToHold.BookCategory));
        CheckRule(new RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(_patronType, activeHolds));
        CheckRule(new PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule(overdueCheckouts));

        var period = GetPeriod();
        AddDomainEvent(new BookPlacedOnHoldDomainEvent(bookToHold.BookId, Id, bookToHold.LibraryBranchId, period));
    }

    public void CancelHold(BookOnHold bookOnHold, List<ActiveHold> activeHolds)
    {
        CheckRule(new PatronCannotCancelHoldOwnedByAnotherPatronRule(Id, bookOnHold));
        CheckRule(new PatronCannotCancelNonExistingHoldRule(bookOnHold, activeHolds));

        AddDomainEvent(new BookHoldCanceledDomainEvent(bookOnHold.BookId, bookOnHold.PatronId, bookOnHold.LibraryBranchId));
    }

    private HoldPeriod GetPeriod()
    {
        switch (_patronType)
        {
            case PatronType.Regular:
                return HoldPeriod.Weekly;
            case PatronType.Researcher:
                return HoldPeriod.Unlimited;
            default:
                throw new ArgumentException($"Unrecognized {nameof(PatronType)}: {_patronType}");
        }
    }
}