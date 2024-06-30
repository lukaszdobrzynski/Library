using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.RestoreBookAvailability;

public class MarkBookAvailableForReservationCommand : InternalCommandBase
{
    protected override string Name => nameof(MarkBookAvailableForReservationCommand);
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public Guid PatronId { get; set; }
    
    public static MarkBookAvailableForReservationCommand CreateSubmitted(Guid bookId, Guid libraryBranchId, Guid patronId)
    {
        return new MarkBookAvailableForReservationCommand
        {
            Id = Guid.NewGuid().ToString(),
            BookId = bookId,
            LibraryBranchId = libraryBranchId,
            PatronId = patronId,
            Status = InternalCommandStatus.Submitted,
            CreatedAt = DateTime.Now
        };
    }
}