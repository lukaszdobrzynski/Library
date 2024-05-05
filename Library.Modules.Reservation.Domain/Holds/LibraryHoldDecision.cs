using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds.Events;

namespace Library.Modules.Reservation.Domain.Holds;

public class LibraryHoldDecision : Entity
{
    public LibraryHoldDecisionId Id { get; private set; }
    public HoldId HoldId { get; private set; }
    public LibraryHoldDecisionStatus DecisionStatus { get; private set; }
    
    private LibraryHoldDecision(HoldId holdId)
    {
        HoldId = holdId;
        DecisionStatus = LibraryHoldDecisionStatus.NoDecision;
        Id = new LibraryHoldDecisionId(Guid.NewGuid());
    }

    public static LibraryHoldDecision NoDecision(HoldId holdId) => new(holdId);

    public void Grant()
    {
        DecisionStatus = LibraryHoldDecisionStatus.Granted;
        AddDomainEvent(new GrantHoldLibraryDecisionAppliedDomainEvent(HoldId));
    }

    public void Cancel()
    {
        DecisionStatus = LibraryHoldDecisionStatus.Cancelled;
        AddDomainEvent(new CancelHoldLibraryDecisionAppliedDomainEvent(HoldId));
    }

    public void Loan()
    {
        DecisionStatus = LibraryHoldDecisionStatus.Loaned;
        AddDomainEvent(new LoanHoldLibraryDecisionAppliedDomainEvent(HoldId));
    }

    public void Reject()
    {
        DecisionStatus = LibraryHoldDecisionStatus.Rejected;
        AddDomainEvent(new LibraryRejectHoldDecisionAppliedDomainEvent(HoldId));
    }

    public void TagReadyToPick()
    {
        DecisionStatus = LibraryHoldDecisionStatus.ReadyToPick;
        AddDomainEvent(new HoldReadyToPickLibraryDecisionAppliedDomainEvent(HoldId));
    } 
}