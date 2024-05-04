using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Checkouts;

public class CheckoutId : TypedIdBase
{
    public CheckoutId(Guid value) : base(value)
    {
    }
}