using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public interface IHoldRepository
{
    Task<Hold> GetByIdAsync(HoldId holdId);
    Task<Hold> GetByRequestHoldId(HoldRequestId holdRequestId);
    Task<bool> ActiveHoldExistsByBookIdAsync(BookId bookId, PatronId patronId);
    Task<List<Hold>> GetWeeklyActiveHoldsByPatronIdAsync(PatronId patronId);
    Task AddAsync(Hold hold);
}