using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class LibraryHoldDecisionId : TypedIdBase
{
    public LibraryHoldDecisionId(Guid value) : base(value)
    {
    }
}