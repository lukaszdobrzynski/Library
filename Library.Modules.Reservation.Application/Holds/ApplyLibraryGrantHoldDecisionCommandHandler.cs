using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds;

public class ApplyLibraryGrantHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyLibraryGrantHoldDecisionCommand>
{
    public async Task Handle(ApplyLibraryGrantHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyLibraryGrantDecision();
    }
}