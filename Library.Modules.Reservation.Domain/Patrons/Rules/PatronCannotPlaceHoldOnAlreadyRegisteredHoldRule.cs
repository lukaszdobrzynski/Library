using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules;

public class PatronCannotPlaceHoldOnAlreadyRegisteredHoldRule : IBusinessRule
{
    private readonly List<ActiveHold> _holds;
    private readonly BookId _bookIdToHold;

    public PatronCannotPlaceHoldOnAlreadyRegisteredHoldRule(List<ActiveHold> holds, BookId bookIdToHold)
    {
        _holds = holds;
        _bookIdToHold = bookIdToHold;
    }

    public bool IsBroken() => _holds.Any(x => x.BookId == _bookIdToHold);

    public string Message => "Patron cannot place hold on the already registered hold";
}