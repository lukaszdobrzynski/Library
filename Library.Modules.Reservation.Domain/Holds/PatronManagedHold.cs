using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class PatronManagedHold : Entity
{
    public PatronManagedHoldId Id { get; private set; }
    public HoldId HoldId { get; private set; }
    public PatronManagedHoldStatus Status { get; private set; }

    private PatronManagedHold(HoldId holdId)
    {
        Status = PatronManagedHoldStatus.Pending;
        HoldId = holdId;
    }

    public static PatronManagedHold CreatePending(HoldId holdId) => new(holdId);
}