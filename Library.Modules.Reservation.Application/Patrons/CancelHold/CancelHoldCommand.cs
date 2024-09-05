using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class CancelHoldCommand(Guid patronId,  Guid holdId) : CommandBase
{
    public Guid PatronId { get; private set; } = patronId;
    public Guid HoldId { get; private set; } = holdId;
}