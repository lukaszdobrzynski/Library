using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Patrons.Events;

public class PatronCreatedDomainEvent(PatronId patronId) : DomainEventBase
{
    public PatronId PatronId { get; private set; } = patronId;
}