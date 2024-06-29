using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class HoldRequestId : TypedIdBase
{
    public HoldRequestId(Guid value) : base(value)
    {
    }
}