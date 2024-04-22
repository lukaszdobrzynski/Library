using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Patrons.Events;

public class BookHoldCanceledDomainEvent : DomainEventBase
{
    public BookId BookId { get; set; }
    public PatronId PatronId { get; set; }
    public LibraryBranchId LibraryBranchId { get; set; }

    public BookHoldCanceledDomainEvent(BookId bookId, PatronId patronId, LibraryBranchId libraryBranchId)
    {
        BookId = bookId;
        PatronId = patronId;
        LibraryBranchId = libraryBranchId;
    }
}