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
    
    public Task<Hold> GetByIdAsync(HoldId holdId)
    {
        return Task.FromResult(Hold.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid()), new PatronId(Guid.NewGuid()), HoldPeriod.Weekly));
    }

    public Task<List<Hold>> GetWeeklyHoldsByPatronIdAsync(PatronId patronId)
    {
        var weekAgo = DateTime.UtcNow.AddDays(-7).Date;
        return _reservationContext.Holds
            .Include(x => x.LibraryHoldDecision)
            .Include(x => x.PatronHoldDecision)
            .Where(x => x.PatronId == patronId && x.CreatedAt.Date >= weekAgo)
            .ToListAsync();
    }

    public async Task AddAsync(Hold hold)
    {
        await _reservationContext.Holds.AddAsync(hold);
    }
}