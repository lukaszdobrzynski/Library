using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds.Rules;

namespace Library.Modules.Reservation.Domain.Holds;

public class ActiveHolds : ValueObject
{
    public int Count { get; private set; }

    private ActiveHolds(int count)
    {
        Count = count;
    }

    public static ActiveHolds Create(int count)
    {
        CheckRule(new ActiveHoldsCountCannotBeNegative(count));
        return new ActiveHolds(count);
    }
}