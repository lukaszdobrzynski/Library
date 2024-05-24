using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public class WeeklyHolds : ValueObject
{
    public PatronId PatronId { get; private set; }
    public IReadOnlyCollection<HoldId> Holds { get; private set; }

    private WeeklyHolds(PatronId patronId, List<HoldId> holds)
    {
        PatronId = patronId;
        Holds = holds ?? [];
    }

    public static WeeklyHolds Create(PatronId patronId, List<HoldId> holds)
    {
        return new WeeklyHolds(patronId, holds);
    }
}