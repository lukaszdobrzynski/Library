using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.LibraryBranch;

namespace Library.Modules.Lending.Domain.Patron;

public class ResearcherPatron : Entity, IAggregateRoot
{
    public PatronId Id { get; private set; }
    
    private List<OverdueCheckout> _overdueCheckouts;

    private ResearcherPatron(PatronId id)
    {
        Id = id;
        _overdueCheckouts = new List<OverdueCheckout>();
    }

    public static ResearcherPatron Create(Guid id)
    {
        return new ResearcherPatron(new PatronId(id));
    }
    
    public int NumberOfOverdueCheckoutsAt(LibraryBranchId libraryBranchId) =>
        _overdueCheckouts.Count(x => x.LibraryBranchId == libraryBranchId);
}