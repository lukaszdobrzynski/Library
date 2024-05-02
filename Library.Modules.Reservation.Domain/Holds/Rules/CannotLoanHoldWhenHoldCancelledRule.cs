using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotLoanHoldWhenHoldCancelledRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotLoanHoldWhenHoldCancelledRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Cancelled;
    public string Message => "Cannot loan a cancelled hold.";
}