using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds;

public class ApplyLibraryHoldReadyToPickDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyLibraryHoldReadyToPickDecisionCommand>
{
    public async Task Handle(ApplyLibraryHoldReadyToPickDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyLibraryReadyToPickDecision();
    }
}