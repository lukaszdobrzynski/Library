using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Books;

public class BookId : TypedIdBase
{
    public BookId(Guid value) : base(value)
    {
    }
}