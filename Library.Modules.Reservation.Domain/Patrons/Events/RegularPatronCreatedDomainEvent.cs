using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Patrons.Events;

public class RegularPatronCreatedDomainEvent(PatronId patronId) : DomainEventBase
{
    public PatronId PatronId { get; private set; } = patronId;
}