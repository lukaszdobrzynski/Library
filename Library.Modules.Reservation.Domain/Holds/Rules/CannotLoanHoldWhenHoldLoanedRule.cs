using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotLoanHoldWhenHoldLoanedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotLoanHoldWhenHoldLoanedRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Loaned;
    public string Message => "Cannot loan a hold when hold is loaned.";
}