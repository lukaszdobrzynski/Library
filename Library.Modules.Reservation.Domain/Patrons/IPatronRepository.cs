namespace Library.Modules.Reservation.Domain.Patrons;

public interface IPatronRepository
{
   Task<RegularPatron> GetByIdAsync(PatronId patronId);
}