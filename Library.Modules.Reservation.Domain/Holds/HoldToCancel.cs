using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public class HoldToCancel : ValueObject
{
    public PatronId OwningPatronId { get; private set; }

    public HoldId HoldId { get; private set; }

    public BookId BookId { get; private set; }

    public LibraryBranchId LibraryBranchId { get; private set; }

    public HoldStatus Status { get; private set; }
    

    private HoldToCancel(PatronId patronId, HoldId holdId, BookId bookId, LibraryBranchId libraryBranchId, HoldStatus status)
    {
        OwningPatronId = patronId;
        HoldId = holdId;
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        Status = status;
    }

    public static HoldToCancel Create(PatronId patronId, HoldId holdId, BookId bookId, LibraryBranchId libraryBranchId, HoldStatus status)
    {
        return new HoldToCancel(patronId, holdId, bookId, libraryBranchId, status);
    }
}