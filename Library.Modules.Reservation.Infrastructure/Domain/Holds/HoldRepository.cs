using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Infrastructure.Domain.Holds;

public class HoldRepository : IHoldRepository
{
    public Task<Hold> GetByIdAsync(HoldId holdId)
    {
        return Task.FromResult(Hold.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid())));
    }

    public Task<List<ActiveHold>> GetActiveHoldsByPatronIdAsync(PatronId patronId)
    {
        return Task.FromResult(new List<ActiveHold> { ActiveHold.Create(new BookId(Guid.NewGuid())) });
    }

    public Task AddAsync(Hold hold)
    {
        return Task.CompletedTask;
    }
}