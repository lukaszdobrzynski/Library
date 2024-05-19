using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotTagHoldReadyToPickWhenHoldRejectedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotTagHoldReadyToPickWhenHoldRejectedRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.Rejected;
    public string Message => "Cannot tag hold ready to pick when hold is rejected.";
}