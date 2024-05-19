using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotLoanHoldWhenHoldGrantedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotLoanHoldWhenHoldGrantedRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.Granted;
    public string Message => "Cannot loan a granted hold.";
}