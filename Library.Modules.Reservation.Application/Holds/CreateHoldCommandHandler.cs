using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Application.Holds;

public class CreateHoldCommandHandler(IHoldRepository holdRepository) : ICommandHandler<CreateHoldCommand>
{
    public async Task Handle(CreateHoldCommand command, CancellationToken cancellationToken)
    {
        var hold = Hold.Create(new BookId(command.BookId), new LibraryBranchId(command.LibraryBranchId));

        await holdRepository.AddAsync(hold);
    }
}