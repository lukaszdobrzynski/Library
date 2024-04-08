using Library.BuildingBlocks.Domain;

namespace Library.Modules.Lending.Domain.Book;

public class BookId : TypedIdBase
{
    public BookId(Guid value) : base(value)
    {
    }
}