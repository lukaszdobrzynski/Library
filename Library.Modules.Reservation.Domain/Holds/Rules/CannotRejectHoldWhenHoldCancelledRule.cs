using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotRejectHoldWhenHoldCancelledRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotRejectHoldWhenHoldCancelledRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Cancelled;
    public string Message => "Cannot reject hold when hold is cancelled.";
}