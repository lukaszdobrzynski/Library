using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Patrons.Rules;

public class PatronCannotPlaceHoldWhenBookOnActiveHoldRule : IBusinessRule
{
    private readonly BookToHold _bookToHold;
    
    public PatronCannotPlaceHoldWhenBookOnActiveHoldRule(BookToHold bookToHold)
    {
        _bookToHold = bookToHold;
    }

    public bool IsBroken() => _bookToHold.IsOnActiveHold;
    public string Message => "Patron cannot place hold on existing hold.";
}