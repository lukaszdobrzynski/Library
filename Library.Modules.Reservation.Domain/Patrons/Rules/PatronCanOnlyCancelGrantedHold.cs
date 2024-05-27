using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules;

public class PatronCanOnlyCancelGrantedHold : IBusinessRule
{
    private readonly HoldToCancel _holdToCancel;
    
    public PatronCanOnlyCancelGrantedHold(HoldToCancel holdToCancel)
    {
        _holdToCancel = holdToCancel;
    }

    public bool IsBroken() => _holdToCancel.Status == HoldStatus.Granted;

    public string Message => "Patron can only cancel granted hold.";
}