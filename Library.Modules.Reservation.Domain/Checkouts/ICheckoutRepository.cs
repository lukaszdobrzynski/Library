using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Checkouts;

public interface ICheckoutRepository
{
    Task<List<Checkout>> GetOverdueCheckoutsByPatronIdAsync(PatronId patronId);
}