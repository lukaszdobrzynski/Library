using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class CancelExpiredHoldCommand : InternalCommandBase
{
    public Guid HoldId { get; set; }
    public Guid BookId { get; set; }
}