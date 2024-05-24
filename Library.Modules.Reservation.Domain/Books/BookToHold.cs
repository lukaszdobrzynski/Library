using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Books;

public class BookToHold : ValueObject
{
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public BookCategory BookCategory { get; private set; }
    public bool IsOnActiveHold { get; private set; }

    private BookToHold(BookId bookId, LibraryBranchId libraryBranchId, BookCategory bookCategory, bool isOnActiveHold)
    {
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        BookCategory = bookCategory;
        IsOnActiveHold = isOnActiveHold;
    }

    public static BookToHold Create(BookId bookId, LibraryBranchId libraryBranchId, BookCategory bookCategory, bool isOnActiveHold)
    {
        return new BookToHold(bookId, libraryBranchId, bookCategory, isOnActiveHold);
    }
}