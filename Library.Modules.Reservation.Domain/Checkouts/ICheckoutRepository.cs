using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Checkouts;

public interface ICheckoutRepository
{
    Task<List<OverdueCheckout>> GetOverdueCheckoutsByPatronIdAsync(PatronId patronId);
}