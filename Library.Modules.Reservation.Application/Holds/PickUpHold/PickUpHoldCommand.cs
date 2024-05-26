using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.PickUpHold;

public class PickUpHoldCommand(Guid holdId) : CommandBase
{
    public Guid HoldId { get; private set; } = holdId;
}