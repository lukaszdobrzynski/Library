using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Events;

public class HoldCreatedDomainEvent(HoldId holdId) : DomainEventBase
{
    public HoldId HoldId { get; } = holdId;
}