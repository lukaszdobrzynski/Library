using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.RejectHold;

public class ApplyRejectHoldDecisionCommand : InternalCommandBase
{
    public Guid RequestHoldId { get; set; }
}