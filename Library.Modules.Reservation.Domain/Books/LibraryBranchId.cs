using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Books;

public class LibraryBranchId : TypedIdBase
{
    public LibraryBranchId(Guid value) : base(value)
    {
    }
}