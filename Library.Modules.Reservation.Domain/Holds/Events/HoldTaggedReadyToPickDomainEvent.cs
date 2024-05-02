using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Events;

public class HoldTaggedReadyToPickDomainEvent(HoldId holdId) : DomainEventBase
{
    public HoldId HoldId { get; private set; } = holdId;
}