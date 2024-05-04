using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Patrons;

public class CancelHoldCommand : CommandBase
{
    public Guid BookId { get; private set; }
    public Guid LibraryBranchId { get; private set; }
    public Guid PatronId { get; private set; }

    private CancelHoldCommand(Guid bookId, Guid libraryBranchId, Guid patronId)
    {
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronId = patronId;
    }
}