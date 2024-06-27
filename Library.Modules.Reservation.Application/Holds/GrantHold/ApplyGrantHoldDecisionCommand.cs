using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class ApplyGrantHoldDecisionCommand(Guid holdId) : InternalCommandBase
{
    public Guid HoldId { get; set; } = holdId;
}