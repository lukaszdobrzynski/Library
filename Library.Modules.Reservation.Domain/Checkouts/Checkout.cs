using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Checkouts;

public class Checkout : Entity, IAggregateRoot
{
    public CheckoutId Id { get; private set; }
    public PatronId PatronId { get; private set; }
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public DateTime DueDate { get; set; }

    private Checkout()
    {
        // EF only
    }
}