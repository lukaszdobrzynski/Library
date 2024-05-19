using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotLoanHoldWhenHoldPendingRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotLoanHoldWhenHoldPendingRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Pending;
    public string Message => "Cannot loan a pending hold.";
}