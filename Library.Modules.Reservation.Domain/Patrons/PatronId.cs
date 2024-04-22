using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Patrons;

public class PatronId : TypedIdBase
{
    public PatronId(Guid value) : base(value)
    {
    }
}