using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Patrons;

public class OverdueCheckout
{
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }

    private OverdueCheckout(BookId bookId, LibraryBranchId libraryBranchId)
    {
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
    }

    public static OverdueCheckout Create(BookId bookId, LibraryBranchId libraryBranchId) =>
        new(bookId, libraryBranchId);
}