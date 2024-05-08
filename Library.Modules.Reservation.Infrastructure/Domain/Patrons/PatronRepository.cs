using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Infrastructure.Domain.Patrons;

public class PatronRepository : IPatronRepository
{
    public Task<Patron> GetByIdAsync(PatronId patronId)
    {
        return Task.FromResult(Patron.CreateRegular(Guid.NewGuid()));
    }
}