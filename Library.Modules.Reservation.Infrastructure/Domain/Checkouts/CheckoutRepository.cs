using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Checkouts;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Infrastructure.Domain.Checkouts;

public class CheckoutRepository : ICheckoutRepository
{
    public Task<List<OverdueCheckout>> GetOverdueCheckoutsByPatronIdAsync(PatronId patronId)
    {
        return Task.FromResult(new List<OverdueCheckout>
            { OverdueCheckout.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid())) });
    }
}