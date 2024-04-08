using Library.BuildingBlocks.Domain;

namespace Library.Modules.Lending.Domain.Patron;

public class ResearcherPatron : Entity, IAggregateRoot
{
    public PatronId Id { get; private set; }

    private ResearcherPatron(PatronId id)
    {
        Id = id;
    }

    public static ResearcherPatron Create(Guid id)
    {
        return new ResearcherPatron(new PatronId(id));
    }
}