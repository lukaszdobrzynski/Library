using Library.BuildingBlocks.Domain;

namespace Library.Modules.Lending.Domain.LibraryBranch;

public class LibraryBranchId : TypedIdBase
{
    public LibraryBranchId(Guid value) : base(value)
    {
    }
}