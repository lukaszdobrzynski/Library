using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds.Rules;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public class WeeklyActiveHolds : ValueObject
{
    public PatronId PatronId { get; private set; }
    public int Count { get; private set; }

    private WeeklyActiveHolds(PatronId patronId, int count)
    {
        PatronId = patronId;
        Count = count;
    }

    public static WeeklyActiveHolds Create(PatronId patronId, int count)
    {
        CheckRule(new WeeklyActiveHoldsCountCannotBeNegative(count));
        return new WeeklyActiveHolds(patronId, count);
    }
}