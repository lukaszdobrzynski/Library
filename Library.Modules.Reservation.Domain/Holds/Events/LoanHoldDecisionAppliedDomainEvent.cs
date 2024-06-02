using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Events;

public class LoanHoldDecisionAppliedDomainEvent(HoldId holdId) : DomainEventBase
{
    public HoldId HoldId { get; } = holdId;
}