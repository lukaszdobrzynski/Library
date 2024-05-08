using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Infrastructure.Domain.Books;

public class BookRepository : IBookRepository
{
    public Task<Book> GetByIdAsync(BookId bookId)
    {
        return Task.FromResult(new Book());
    }
}