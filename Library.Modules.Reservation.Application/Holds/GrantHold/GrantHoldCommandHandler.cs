using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class GrantHoldCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<GrantHoldCommand>
{
    public async Task Handle(GrantHoldCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyGrantDecision();
    }
}