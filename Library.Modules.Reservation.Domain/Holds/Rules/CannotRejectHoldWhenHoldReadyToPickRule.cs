using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotRejectHoldWhenHoldReadyToPickRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotRejectHoldWhenHoldReadyToPickRule(PatronHoldDecision patronHoldDecision, LibraryHoldDecision libraryHoldDecision)
    {
        _holdStatus = HoldStatus.From(patronHoldDecision.DecisionStatus, libraryHoldDecision.DecisionStatus);
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.ReadyToPick;
    public string Message => "Cannot reject hold when hold is ready to pick.";
}