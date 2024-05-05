using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds;
 
public class ApplyLibraryRejectHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyLibraryRejectHoldDecisionCommand>
{
    public async Task Handle(ApplyLibraryRejectHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyLibraryRejectDecision();
    }
}