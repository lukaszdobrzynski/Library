using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class ApplyLibraryCancelHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyLibraryCancelHoldDecisionCommand>
{
    public async Task Handle(ApplyLibraryCancelHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyLibraryCancelDecision();
    }
}