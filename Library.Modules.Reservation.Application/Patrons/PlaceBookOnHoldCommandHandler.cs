using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Checkouts;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Application.Patrons;

public class PlaceBookOnHoldCommandHandler(
    IPatronRepository patronRepository,
    IBookRepository bookRepository,
    IHoldRepository holdRepository,
    ICheckoutRepository checkoutRepository)
    : ICommandHandler<PlaceBookOnHoldCommand>
{
    public async Task Handle(PlaceBookOnHoldCommand command, CancellationToken cancellationToken)
    {
        var patron = await patronRepository.GetByIdAsync(new PatronId(command.PatronId));
        var book = await bookRepository.GetByIdAsync(new BookId(command.BookId));
        var holds = await holdRepository.GetActiveHoldsByPatronIdAsync(new PatronId(command.PatronId));
        var checkouts =
            await checkoutRepository.GetOverdueCheckoutsByPatronIdAsync(new PatronId(command.PatronId));
        
        var bookToHold = BookToHold.Create(book.Id, book.LibraryBranchId, patron.Id, book.BookCategory);
        var activeHolds = holds.Select(x => ActiveHold.Create(x.BookId)).ToList();
        var overdueCheckouts = checkouts.Select(x => 
                OverdueCheckout.Create(x.BookId, x.LibraryBranchId))
            .ToList();
        
        patron.PlaceOnHold(bookToHold, activeHolds, overdueCheckouts);
    }
}