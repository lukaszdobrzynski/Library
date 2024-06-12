using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.CancelHold;

public class CancelHoldCommand : InternalCommandBase
{
    protected override string Name => nameof(CancelHoldCommand);
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public Guid PatronId { get; set; }
    
    public static CancelHoldCommand CreateSubmitted(Guid bookId, Guid libraryBranchId, Guid patronId)
    {
        return new CancelHoldCommand
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