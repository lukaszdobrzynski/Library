using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds.LoanHold;

public class LoanHoldCommandHandler(IHoldRepository holdRepository)
    : ICommandHandler<LoanHoldCommand>
{
    public async Task Handle(LoanHoldCommand command, CancellationToken cancellationToken)
    {
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        hold.ApplyLoanDecision();
    }
}