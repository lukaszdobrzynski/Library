using Library.Modules.Reservation.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure.Domain.Books;

public class BookRepository : IBookRepository
{
    private readonly ReservationContext _reservationContext;
    
    public BookRepository(ReservationContext reservationContext)
    {
        _reservationContext = reservationContext;
    }
    public Task<Book> GetByIdAsync(BookId bookId)
    {
        return _reservationContext.Books.SingleOrDefaultAsync(x => x.Id == bookId);
    }
}