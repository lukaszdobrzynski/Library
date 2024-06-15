using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class RequestHoldCommand : InternalCommandBase
{
    protected override string Name => nameof(RequestHoldCommand);
    
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public Guid PatronId { get; set; }
    public DateTime? Till { get; set; }

    public static RequestHoldCommand CreateSubmitted(Guid bookId, Guid libraryBranchId, Guid patronId, DateTime? till)
    {
        return new RequestHoldCommand
        {
            Id = Guid.NewGuid().ToString(),
            BookId = bookId,
            LibraryBranchId = libraryBranchId,
            PatronId = patronId,
            Till = till,
            CreatedAt = DateTime.UtcNow,
            Status = InternalCommandStatus.Submitted
        };
    }
}