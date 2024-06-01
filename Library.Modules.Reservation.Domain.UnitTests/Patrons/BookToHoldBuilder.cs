using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.UnitTests.Patrons;

public class BookToHoldBuilder
{
    private readonly BookId _bookId;
    private readonly LibraryBranchId _libraryBranchId;
    private readonly BookCategory _bookCategory;
    private bool _isOnActiveHold;
    
    private BookToHoldBuilder(BookId bookId, LibraryBranchId libraryBranchId, BookCategory bookCategory)
    {
        _bookId = bookId;
        _libraryBranchId = libraryBranchId;
        _bookCategory = bookCategory;
    }

    public static BookToHoldBuilder InitCirculating(BookId bookId, LibraryBranchId libraryBranchId) => 
        new(bookId, libraryBranchId, BookCategory.Circulating);

    public static BookToHoldBuilder InitRestricted(BookId bookId, LibraryBranchId libraryBranchId) =>
        new(bookId, libraryBranchId, BookCategory.Restricted);

    public BookToHoldBuilder OnActiveHold()
    {
        _isOnActiveHold = true;
        return this;
    }
    
    public BookToHold Build()
    {
        return BookToHold.Create(_bookId, _libraryBranchId, _bookCategory, _isOnActiveHold);
    }
}