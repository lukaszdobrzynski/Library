using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class HoldId : TypedIdBase
{
    public HoldId(Guid value) : base(value)
    {
    }
}