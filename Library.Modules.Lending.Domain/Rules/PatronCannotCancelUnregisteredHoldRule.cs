using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Book;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Rules;

public class PatronCannotCancelUnregisteredHoldRule : IBusinessRule
{
    private BookOnHold _bookOnHold;
    private List<Hold> _patronHolds;
    
    public PatronCannotCancelUnregisteredHoldRule(BookOnHold bookOnHold, List<Hold> patronHolds)
    {
        _bookOnHold = bookOnHold;
        _patronHolds = patronHolds;
    }

    public bool IsBroken() => 
        _patronHolds.Select(x => x.BookId).Contains(_bookOnHold.BookId) == false;

    public string Message => "Patron cannot cancel unregistered hold.";
}