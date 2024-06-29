using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class ApplyGrantHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyGrantHoldDecisionCommand>
{
    public async Task Handle(ApplyGrantHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByRequestHoldId(new HoldRequestId(command.RequestHoldId));
        hold.ApplyGrantDecision();
    }
}