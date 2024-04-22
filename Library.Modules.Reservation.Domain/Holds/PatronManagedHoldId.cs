using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class PatronManagedHoldId : TypedIdBase
{
    public PatronManagedHoldId(Guid value) : base(value)
    {
    }
}