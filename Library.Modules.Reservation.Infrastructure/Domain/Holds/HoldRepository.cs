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
        return Task.FromResult(Hold.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid())));
    }

    public async Task<List<Hold>> GetActiveHoldsByPatronIdAsync(PatronId patronId)
    {
        return await _reservationContext.Holds
            .Include(x => x.LibraryHoldDecision)
            .Include(x => x.PatronHoldDecision)
            .Where(x => x.PatronId == patronId && (x.Status.Value == HoldStatus.Granted || 
                                                   x.Status.Value == HoldStatus.Pending || 
                                                   x.Status.Value == HoldStatus.ReadyToPick))
            .ToListAsync();
    }

    public Task AddAsync(Hold hold)
    {
        return Task.CompletedTask;
    }
}