using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotTagHoldReadyToPickWhenHoldCancelledRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotTagHoldReadyToPickWhenHoldCancelledRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.Cancelled;
    public string Message => "Cannot tag hold ready to pick when hold is cancelled.";
}