using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotLoanHoldWhenHoldGrantedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotLoanHoldWhenHoldGrantedRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.Granted;
    public string Message => "Cannot loan a granted hold.";
}