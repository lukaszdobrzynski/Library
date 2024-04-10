using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Book;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Rules;

public class PatronCannotCancelHoldPlacedByAnotherPatronRule : IBusinessRule
{
    private PatronId _patronId;

    private BookOnHold _bookOnHold;
    
    public PatronCannotCancelHoldPlacedByAnotherPatronRule(PatronId patronId, BookOnHold bookOnHold)
    {
        _patronId = patronId;
        _bookOnHold = bookOnHold;
    }
    
    public bool IsBroken()
    {
        return _patronId != _bookOnHold.PatronId;
    }

    public string Message => "Patron cannot cancel a hold placed by another patron.";
}