using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Book;

public class BookOnHold : ValueObject
{
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public PatronId PatronId { get; private set; }
    
    private BookOnHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronId = patronId;
    }

    public static BookOnHold Create(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        return new BookOnHold(bookId, libraryBranchId, patronId);
    }
}