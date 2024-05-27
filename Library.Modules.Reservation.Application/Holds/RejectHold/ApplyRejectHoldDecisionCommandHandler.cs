using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.RejectHold;
 
public class ApplyRejectHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyRejectHoldDecisionCommand>
{
    public async Task Handle(ApplyRejectHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyRejectDecision();
    }
}