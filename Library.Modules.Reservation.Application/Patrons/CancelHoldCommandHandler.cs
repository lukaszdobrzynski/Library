﻿using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Application.Patrons;

public class CancelHoldCommandHandler(IPatronRepository patronRepository, IHoldRepository holdRepository)
    : ICommandHandler<CancelHoldCommand>
{
    public async Task Handle(CancelHoldCommand command, CancellationToken cancellationToken)
    {
        var patron = await patronRepository.GetByIdAsync(new PatronId(command.PatronId));
        var hold = await holdRepository.GetByIdAsync(new HoldId(command.HoldId));
        
        patron.CancelHold(hold);
    }
}