using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class LibraryManagedHoldId : TypedIdBase
{
    public LibraryManagedHoldId(Guid value) : base(value)
    {
    }
}