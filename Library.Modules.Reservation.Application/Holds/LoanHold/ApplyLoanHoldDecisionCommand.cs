using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.LoanHold;

public class ApplyLoanHoldDecisionCommand(Guid holdId) : CommandBase
{
    public Guid HoldId { get; private set; } = holdId;
}