namespace Library.Modules.Reservation.Domain.Books;

public interface IBookRepository
{
    Task<Book> GetByIdAsync(BookId bookId);
}