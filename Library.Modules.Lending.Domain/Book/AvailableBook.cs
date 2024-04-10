using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Book;

public class AvailableBook : ValueObject
{
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public PatronId PatronId { get; private set; }
    public BookCategory BookCategory { get; set; }
    
    private AvailableBook(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, BookCategory bookCategory)
    {
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronId = patronId;
        BookCategory = bookCategory;
    }

    public static AvailableBook Create(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, BookCategory bookCategory)
    {
        return new AvailableBook(bookId, libraryBranchId, patronId, bookCategory);
    }
}