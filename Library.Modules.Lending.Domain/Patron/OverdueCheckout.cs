using Library.Modules.Lending.Domain.Book;
using Library.Modules.Lending.Domain.LibraryBranch;

namespace Library.Modules.Lending.Domain.Patron;

public class OverdueCheckout(BookId bookId, LibraryBranchId libraryBranchId)
{
    public BookId BookId { get; set; } = bookId;
    public LibraryBranchId LibraryBranchId { get; set; } = libraryBranchId;
}