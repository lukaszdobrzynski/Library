using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class ApplyPatronCancelHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyPatronCancelHoldDecisionCommand>
{
    public async Task Handle(ApplyPatronCancelHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyPatronCancelDecision();
    }
}