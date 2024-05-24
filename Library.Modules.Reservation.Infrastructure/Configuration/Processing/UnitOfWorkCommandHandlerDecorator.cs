using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Application.Contracts;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class UnitOfWorkCommandHandlerDecorator<T>(IRequestHandler<T> decorated, IUnitOfWork unitOfWork)
    : IRequestHandler<T>
    where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await decorated.Handle(command, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}