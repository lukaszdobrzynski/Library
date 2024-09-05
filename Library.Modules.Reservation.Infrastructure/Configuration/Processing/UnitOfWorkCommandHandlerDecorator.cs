using Library.Modules.Reservation.Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

internal class UnitOfWorkCommandHandlerDecorator<T>(IRequestHandler<T> decorated, IUnitOfWork unitOfWork, ReservationContext reservationContext)
    : IRequestHandler<T>
    where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        if (command is InternalCommandBase)
        {
            var internalCommand = await reservationContext.InternalCommands
                .FirstOrDefaultAsync(x => x.Id == command.Id, 
                    cancellationToken: cancellationToken);
            
            if (internalCommand != null)
            {
                internalCommand.ProcessedAt = DateTime.UtcNow;
            }
        }
        
        await decorated.Handle(command, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}