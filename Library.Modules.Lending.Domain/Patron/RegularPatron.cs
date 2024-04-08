using Library.BuildingBlocks.Domain;

namespace Library.Modules.Lending.Domain.Patron;

public class RegularPatron : Entity, IAggregateRoot
{
    public PatronId Id { get; private set; }

    private RegularPatron(PatronId id)
    {
        Id = id;
    }

    public static RegularPatron Create(Guid id)
    {
        return new RegularPatron(new PatronId(id));
    }
}