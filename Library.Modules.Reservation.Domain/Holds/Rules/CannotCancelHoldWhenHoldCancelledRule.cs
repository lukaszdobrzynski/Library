using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotCancelHoldWhenHoldCancelledRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotCancelHoldWhenHoldCancelledRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Cancelled;
    public string Message => "Cannot cancel hold when hold is cancelled.";
}