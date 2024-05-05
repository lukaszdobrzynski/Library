using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Events;

public class LoanHoldLibraryDecisionAppliedDomainEvent(HoldId holdId) : DomainEventBase
{
    public HoldId HoldId { get; set; } = holdId;
}