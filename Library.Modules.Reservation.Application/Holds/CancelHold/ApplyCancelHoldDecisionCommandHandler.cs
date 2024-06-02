using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class ApplyCancelHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyCancelHoldDecisionCommand>
{
    public async Task Handle(ApplyCancelHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyCancelDecision();
    }
}