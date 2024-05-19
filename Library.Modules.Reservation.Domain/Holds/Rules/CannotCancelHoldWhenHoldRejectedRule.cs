using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotCancelHoldWhenHoldRejectedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;
    
    public CannotCancelHoldWhenHoldRejectedRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Rejected;
    public string Message => "Cannot cancel hold when hold is rejected.";
}