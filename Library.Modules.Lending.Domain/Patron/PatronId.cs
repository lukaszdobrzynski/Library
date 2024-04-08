using Library.BuildingBlocks.Domain;

namespace Library.Modules.Lending.Domain.Patron;

public class PatronId : TypedIdBase
{
    public PatronId(Guid value) : base(value)
    {
    }
}