using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotTagHoldReadyToPickWhenHoldLoanedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotTagHoldReadyToPickWhenHoldLoanedRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Loaned;
    public string Message => "Cannot tag hold ready to pick when hold is loaned.";
}