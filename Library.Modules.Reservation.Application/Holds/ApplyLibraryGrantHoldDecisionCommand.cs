using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds;

public class ApplyLibraryGrantHoldDecisionCommand(Guid holdId) : CommandBase
{
    public Guid HoldId { get; set; } = holdId;
}