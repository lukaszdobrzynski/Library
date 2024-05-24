using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds;

public class ApplyLibraryLoanHoldDecisionCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<ApplyLibraryLoanHoldDecisionCommand>
{
    public async Task Handle(ApplyLibraryLoanHoldDecisionCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyLoanDecision();
    }
}