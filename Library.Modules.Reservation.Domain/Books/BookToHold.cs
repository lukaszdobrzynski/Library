using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Books;

public class BookToHold : ValueObject
{
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public PatronId PatronId { get; private set; }
    public BookCategory BookCategory { get; set; }
    
    private BookToHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, BookCategory bookCategory)
    {
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronId = patronId;
        BookCategory = bookCategory;
    }

    public static BookToHold Create(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, BookCategory bookCategory)
    {
        return new BookToHold(bookId, libraryBranchId, patronId, bookCategory);
    }
}