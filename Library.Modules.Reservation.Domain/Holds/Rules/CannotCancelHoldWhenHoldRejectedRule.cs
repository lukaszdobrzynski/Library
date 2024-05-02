using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotCancelHoldWhenHoldRejectedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotCancelHoldWhenHoldRejectedRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Rejected;
    public string Message => "Cannot cancel hold when hold is rejected.";
}