using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Books;

public class Book : AggregateRootBase
{
    public BookId Id { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public BookCategory BookCategory { get; private set; }
}