using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotLoanHoldWhenHoldRejectedRule : IBusinessRule
{
    private HoldStatus _holdStatus;
    
    public CannotLoanHoldWhenHoldRejectedRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Rejected;
    public string Message => "Cannot loan a rejected hold";
}