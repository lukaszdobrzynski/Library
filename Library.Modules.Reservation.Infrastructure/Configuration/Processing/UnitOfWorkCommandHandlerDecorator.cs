using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class UnitOfWorkCommandHandlerDecorator<T>(ICommandHandler<T> decorated, IUnitOfWork unitOfWork)
    : ICommandHandler<T>
    where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await decorated.Handle(command, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}