using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Events;

public class RegularPatronCreatedDomainEvent(PatronId patronId) : DomainEventBase
{
    public PatronId PatronId { get; set; } = patronId;
}