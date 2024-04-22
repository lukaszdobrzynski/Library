using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Holds;

public class ActiveHold : ValueObject
{
    public BookId BookId { get; }

    private ActiveHold(BookId bookId)
    {
        BookId = bookId;
    }

    public static ActiveHold Create(BookId bookId) => new(bookId);
}