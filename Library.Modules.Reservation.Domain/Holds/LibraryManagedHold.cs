using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class LibraryManagedHold : Entity
{
    public LibraryManagedHoldId Id { get; private set; }
    public HoldId HoldId { get; private set; }
    public LibraryManagedHoldStatus Status { get; private set; }
}