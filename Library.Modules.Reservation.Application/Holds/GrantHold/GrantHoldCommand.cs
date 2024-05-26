using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class GrantHoldCommand(Guid holdId) : CommandBase
{
    public Guid HoldId { get; set; } = holdId;
}