using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Holds;

public class Hold : Entity, IAggregateRoot
{
    public HoldId Id { get; private set; }
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public PatronManagedHold PatronManagedHold { get; private set; }
    public LibraryManagedHold LibraryManagedHold { get; private set; }
    public HoldStatus Status => GetStatus(PatronManagedHold, LibraryManagedHold);

    public bool IsActive =>
        Status == HoldStatus.Pending || 
        Status == HoldStatus.Granted || 
        Status == HoldStatus.ReadyToPick;

    private Hold(BookId bookId, LibraryBranchId libraryBranchId)
    {
        Id = new HoldId(Guid.NewGuid());
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronManagedHold = PatronManagedHold.CreatePending(Id);
    }

    public static Hold CreatePending(BookId bookId, LibraryBranchId libraryBranchId) =>
        new (bookId, libraryBranchId);

    private HoldStatus GetStatus(PatronManagedHold patronManagedHold, LibraryManagedHold libraryManagedHold)
    {
        if (libraryManagedHold is null)
        {
            return HoldStatus.From(patronManagedHold.Status);
        }

        return string.Equals(patronManagedHold.Status, libraryManagedHold.Status) ? 
            HoldStatus.From(patronManagedHold.Status) : 
            HoldStatus.From(libraryManagedHold.Status);
    }
}