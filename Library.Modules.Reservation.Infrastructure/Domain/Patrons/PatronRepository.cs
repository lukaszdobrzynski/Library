using Library.Modules.Reservation.Domain.Patrons;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure.Domain.Patrons;

internal class PatronRepository : IPatronRepository
{
    private readonly ReservationContext _reservationContext;
    
    public PatronRepository(ReservationContext reservationContext)
    {
        _reservationContext = reservationContext;
    }
    
    public Task<Patron> GetByIdAsync(PatronId patronId)
    {
        return _reservationContext.Patrons.SingleOrDefaultAsync(x => x.Id == patronId);
    }
}