using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure.Domain.Holds;

public class HoldRepository : IHoldRepository
{
    private readonly ReservationContext _reservationContext;

    public HoldRepository(ReservationContext reservationContext)
    {
        _reservationContext = reservationContext;
    }
    
    public async Task<Hold> GetByIdAsync(HoldId holdId)
    {
        return await _reservationContext.Holds.SingleAsync(x => x.Id == holdId);
    }

    public async Task<bool> ActiveHoldExistsByBookIdAsync(BookId bookId)
    {
        var hold = await _reservationContext.Holds
            .Where(x => x.BookId == bookId && x.IsActive == true)
            .SingleOrDefaultAsync();

        return hold?.IsActive ?? false;
    }

    public Task<List<Hold>> GetWeeklyActiveHoldsByPatronIdAsync(PatronId patronId)
    {
        var today = DateTime.UtcNow.Date;
        var weekAgo = today.AddDays(-7);

        return  _reservationContext.Holds
            .Where(x => x.PatronId == patronId && 
                        x.IsActive == true && 
                        x.CreatedAt >= weekAgo)
            .ToListAsync();
    }

    public async Task AddAsync(Hold hold)
    {
        await _reservationContext.Holds.AddAsync(hold);
    }
}