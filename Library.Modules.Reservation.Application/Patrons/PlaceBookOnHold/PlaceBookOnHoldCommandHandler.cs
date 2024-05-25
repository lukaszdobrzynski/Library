using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Checkouts;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

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
        var isBookOnActiveHold = await holdRepository.ActiveHoldExistsByBookIdAsync(book.Id);
        var checkouts =
            await checkoutRepository.GetOverdueCheckoutsByPatronIdAsync(new PatronId(command.PatronId));
        var weeklyActiveHolds = await holdRepository.GetWeeklyActiveHoldsByPatronIdAsync(new PatronId(command.PatronId));
            
        var bookToHold = BookToHold.Create(book.Id, book.LibraryBranchId, book.BookCategory, isBookOnActiveHold);
        var overdueCheckouts = checkouts.Select(x => 
                OverdueCheckout.Create(x.BookId, x.LibraryBranchId))
            .ToList();
        var weeklyHolds =
            WeeklyHolds.Create(new PatronId(command.PatronId), weeklyActiveHolds.Select(x => x.Id).ToList());
        
        patron.PlaceOnHold(bookToHold, overdueCheckouts, weeklyHolds);
    }
}