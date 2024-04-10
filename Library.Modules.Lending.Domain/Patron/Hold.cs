using Library.Modules.Lending.Domain.Book;
using Library.Modules.Lending.Domain.LibraryBranch;

namespace Library.Modules.Lending.Domain.Patron;

public class Hold(BookId bookId, LibraryBranchId libraryBranchId)
{
    public BookId BookId { get; private set; } = bookId;
    public LibraryBranchId LibraryBranchId { get; private set; } = libraryBranchId;
}