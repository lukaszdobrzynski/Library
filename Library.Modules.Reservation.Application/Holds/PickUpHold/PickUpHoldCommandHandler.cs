using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.PickUpHold;

public class PickUpHoldCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<PickUpHoldCommand>
{
    public async Task Handle(PickUpHoldCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyReadyToPickDecision();
    }
}