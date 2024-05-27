using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.PickUpHold;

public class ApplyHoldReadyToPickDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyHoldReadyToPickDecisionCommand>
{
    public async Task Handle(ApplyHoldReadyToPickDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyReadyToPickDecision();
    }
}