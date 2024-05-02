using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds.Events;

namespace Library.Modules.Reservation.Domain.Holds;

public class PatronHoldDecision : Entity
{
    public PatronHoldDecisionId Id { get; private set; }
    public HoldId HoldId { get; private set; }
    public PatronHoldDecisionStatus DecisionStatus { get; private set; }

    private PatronHoldDecision(HoldId holdId)
    {
        DecisionStatus = PatronHoldDecisionStatus.NoDecision;
        HoldId = holdId;
        Id = new PatronHoldDecisionId(Guid.NewGuid());
    }

    public static PatronHoldDecision NoDecision(HoldId holdId) => new(holdId);

    public void Cancel()
    {
        DecisionStatus = PatronHoldDecisionStatus.Cancelled;
        AddDomainEvent(new HoldCancelledByPatronDomainEvent(HoldId));
    }
}