using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.RejectHold;

public class RejectHoldCommand(Guid holdId) : CommandBase
{
    public Guid HoldId { get; private set; } = holdId;
}