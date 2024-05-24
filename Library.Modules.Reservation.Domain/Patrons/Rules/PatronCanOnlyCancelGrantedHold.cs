using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules;

public class PatronCanOnlyCancelGrantedHold : IBusinessRule
{
    private readonly Hold _hold;
    
    public PatronCanOnlyCancelGrantedHold(Hold hold)
    {
        _hold = hold;
    }

    public bool IsBroken() => _hold.Status == HoldStatus.Granted;

    public string Message => "Patron can only cancel granted hold.";
}