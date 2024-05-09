using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Patrons;

public class CancelHoldCommand(Guid bookId, Guid libraryBranchId, Guid patronId) : CommandBase
{
    public Guid BookId { get; private set; } = bookId;
    public Guid LibraryBranchId { get; private set; } = libraryBranchId;
    public Guid PatronId { get; private set; } = patronId;
}