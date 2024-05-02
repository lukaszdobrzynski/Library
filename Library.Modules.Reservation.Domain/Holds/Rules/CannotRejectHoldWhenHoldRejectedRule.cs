using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotRejectHoldWhenHoldRejectedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotRejectHoldWhenHoldRejectedRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Rejected;
    public string Message => "Cannot reject hold when hold is rejected.";
}