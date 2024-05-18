using Library.Modules.Reservation.Domain.Checkouts;
using Library.Modules.Reservation.Domain.Patrons;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure.Domain.Checkouts;

public class CheckoutRepository : ICheckoutRepository
{
    private readonly ReservationContext _reservationContext;
    
    public CheckoutRepository(ReservationContext reservationContext)
    {
        _reservationContext = reservationContext;
    }
    
    public Task<List<Checkout>> GetOverdueCheckoutsByPatronIdAsync(PatronId patronId)
    {
        return _reservationContext.Checkouts.Where(x => x.PatronId == patronId && x.DueDate.Date < DateTime.UtcNow.Date)
            .ToListAsync();
    }
}