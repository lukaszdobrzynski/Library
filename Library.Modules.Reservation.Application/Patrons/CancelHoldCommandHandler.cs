using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Application.Patrons;

public class CancelHoldCommandHandler(IPatronRepository patronRepository, IHoldRepository holdRepository)
    : ICommandHandler<CancelHoldCommand>
{
    public async Task Handle(CancelHoldCommand command, CancellationToken cancellationToken)
    {
        var patron = await patronRepository.GetByIdAsync(new PatronId(command.PatronId));
        var activeHolds = await holdRepository.GetActiveHoldsByPatronIdAsync(new PatronId(command.PatronId));
        
        var bookOnHold = BookOnHold.Create(new BookId(command.BookId), new LibraryBranchId(command.LibraryBranchId),
            new PatronId(command.PatronId));
        
        patron.CancelHold(bookOnHold, activeHolds);
    }
}