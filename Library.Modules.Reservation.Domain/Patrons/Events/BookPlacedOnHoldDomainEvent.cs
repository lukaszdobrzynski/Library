using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Patrons.Events;

public class BookPlacedOnHoldDomainEvent(BookId bookId, PatronId patronId, LibraryBranchId libraryBranchId)
    : DomainEventBase
{
    public BookId BookId { get; set; } = bookId;
    public PatronId PatronId { get; set; } = patronId;
    public LibraryBranchId LibraryBranchId { get; set; } = libraryBranchId;
}