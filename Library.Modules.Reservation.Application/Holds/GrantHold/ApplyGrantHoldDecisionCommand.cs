using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class ApplyGrantHoldDecisionCommand : InternalCommandBase
{
    public Guid RequestHoldId { get; init; }
}