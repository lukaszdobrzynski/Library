using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds;

public class CreateHoldCommand(Guid bookId, Guid libraryBranchId) : CommandBase
{
    public Guid BookId { get; private set; } = bookId;
    public Guid LibraryBranchId { get; private set; } = libraryBranchId;
}