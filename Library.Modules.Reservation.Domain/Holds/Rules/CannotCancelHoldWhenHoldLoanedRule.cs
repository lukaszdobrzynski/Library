using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotCancelHoldWhenHoldLoanedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotCancelHoldWhenHoldLoanedRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Loaned;
    public string Message => "Cannot cancel hold when hold is loaned.";
}