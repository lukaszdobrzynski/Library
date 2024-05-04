using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds;

public class CreateHoldCommandHandler : ICommandHandler<CreateHoldCommand>
{
    public Task Handle(CreateHoldCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}