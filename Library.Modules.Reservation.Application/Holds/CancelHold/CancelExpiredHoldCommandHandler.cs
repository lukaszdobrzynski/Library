using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class CancelExpiredHoldCommandHandler(IHoldRepository holdRepository) : ICommandHandler<CancelExpiredHoldCommand>
{
    public async Task Handle(CancelExpiredHoldCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        hold.ApplyCancelDecision();
    }
}