using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotLoanHoldWhenHoldPendingRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotLoanHoldWhenHoldPendingRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Pending;
    public string Message => "Cannot loan a pending hold.";
}