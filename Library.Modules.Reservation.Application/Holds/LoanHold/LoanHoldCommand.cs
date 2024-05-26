using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.LoanHold;

public class LoanHoldCommand(Guid holdId) : CommandBase
{
    public Guid HoldId { get; private set; } = holdId;
}