using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotGrantHoldWhenHoldLoanedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotGrantHoldWhenHoldLoanedRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Loaned;
    public string Message => "Cannot grant hold when hold is loaned.";
}