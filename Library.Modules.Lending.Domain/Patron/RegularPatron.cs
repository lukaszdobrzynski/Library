using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Book;
using Library.Modules.Lending.Domain.Events;
using Library.Modules.Lending.Domain.Rules;

namespace Library.Modules.Lending.Domain.Patron;

public class RegularPatron : Entity, IAggregateRoot
{
    private List<Hold> _holds;

    public PatronId Id { get; private set; }
    public IReadOnlyCollection<Hold> Holds => _holds;

    private RegularPatron(PatronId id)
    {
        Id = id;
        _holds = new List<Hold>();
        AddDomainEvent(new RegularPatronCreatedDomainEvent(Id));
    }

    public static RegularPatron Create(Guid id)
    {
        return new RegularPatron(new PatronId(id));
    }

    public void PlaceOnHold(AvailableBook availableBook, List<OverdueCheckout> overdueCheckouts)
    {
        CheckRule(new RegularPatronCannotPlaceHoldOnRestrictedBookRule(availableBook.BookCategory));
        CheckRule(new RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(_holds));
        CheckRule(new PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule(overdueCheckouts));
        
        _holds.Add(new Hold(availableBook.BookId, availableBook.LibraryBranchId));
        AddDomainEvent(new BookPlacedOnHoldDomainEvent(availableBook.BookId, Id, availableBook.LibraryBranchId));
    }

    public void CancelHold(BookOnHold bookOnHold)
    {
        CheckRule(new PatronCannotCancelHoldPlacedByAnotherPatronRule(Id, bookOnHold));
        CheckRule(new PatronCannotCancelUnregisteredHoldRule(bookOnHold, _holds));

        var holdToCancel = _holds.Single(x => x.BookId == bookOnHold.BookId);
        _holds.Remove(holdToCancel);
        
        AddDomainEvent(new BookHoldCanceledDomainEvent(bookOnHold.BookId, bookOnHold.PatronId, bookOnHold.LibraryBranchId));
    }
}