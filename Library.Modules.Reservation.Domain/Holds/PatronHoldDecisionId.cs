using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class PatronHoldDecisionId : TypedIdBase
{
    public PatronHoldDecisionId(Guid value) : base(value)
    {
    }
}