using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.RejectHold;
 
public class RejectHoldCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<RejectHoldCommand>
{
    public async Task Handle(RejectHoldCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyRejectDecision();
    }
}