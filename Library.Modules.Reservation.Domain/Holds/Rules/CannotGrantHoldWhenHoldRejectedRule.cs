using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotGrantHoldWhenHoldRejectedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotGrantHoldWhenHoldRejectedRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Rejected;
    public string Message => "Cannot grant hold when hold is rejected.";
}