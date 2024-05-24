using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public interface IHoldRepository
{
    Task<Hold> GetByIdAsync(HoldId holdId);
    public Task<bool> ActiveHoldExistsByBookIdAsync(BookId bookId);
    public Task<List<Hold>> GetWeeklyActiveHoldsByPatronIdAsync(PatronId patronId);
    Task AddAsync(Hold hold);
}