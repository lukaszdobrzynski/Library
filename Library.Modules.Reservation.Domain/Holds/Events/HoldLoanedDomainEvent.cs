using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Events;

public class HoldLoanedDomainEvent(HoldId holdId) : DomainEventBase
{
    public HoldId HoldId { get; set; } = holdId;
}