using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons.Events;
using Library.Modules.Reservation.Domain.Patrons.Rules;

namespace Library.Modules.Reservation.Domain.Patrons;

public class Patron : AggregateRootBase
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
        IncreaseVersion();
    }

    public static Patron CreateRegular(Guid id)
    {
        return new Patron(new PatronId(id), PatronType.Regular);
    }
    
    public static Patron CreateResearcher(Guid id)
    {
        return new Patron(new PatronId(id), PatronType.Researcher);
    }

    public void PlaceOnHold(BookToHold bookToHold, List<OverdueCheckout> overdueCheckouts, WeeklyHolds weeklyHolds)
    {
        CheckRule(new PatronCannotPlaceHoldOnExistingHoldRule(bookToHold));
        CheckRule(new RegularPatronCannotPlaceHoldOnRestrictedBookRule(_patronType, bookToHold.BookCategory));
        CheckRule(new RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(_patronType, weeklyHolds));
        CheckRule(new PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule(overdueCheckouts));

        var period = GetHoldPeriodForPatron();
        AddDomainEvent(new BookPlacedOnHoldDomainEvent(bookToHold.BookId, Id, bookToHold.LibraryBranchId, period));
        IncreaseVersion();
    }

    public void CancelHold(Hold hold)
    {
        CheckRule(new PatronCannotCancelNonExistingHoldRule(hold));
        CheckRule(new PatronCannotCancelHoldOwnedByAnotherPatronRule(Id, hold));
        CheckRule(new PatronCanOnlyCancelGrantedHold(hold));

        AddDomainEvent(new BookHoldCanceledDomainEvent(hold.BookId, hold.PatronId, hold.LibraryBranchId));
        IncreaseVersion();
    }

    private DateTime? GetHoldPeriodForPatron()
    {
        switch (_patronType)
        {
            case PatronType.Regular:
                return DateTime.UtcNow.Date.AddDays(7);
            case PatronType.Researcher:
                return null;
            default:
                throw new ArgumentException($"Unrecognized {nameof(PatronType)}: {_patronType}");
        }
    }
}