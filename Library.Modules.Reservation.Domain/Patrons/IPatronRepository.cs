namespace Library.Modules.Reservation.Domain.Patrons;

public interface IPatronRepository
{
   Task<Patron> GetByIdAsync(PatronId patronId);
}