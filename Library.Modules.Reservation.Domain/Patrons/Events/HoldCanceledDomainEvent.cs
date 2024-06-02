using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Events;

public class HoldCanceledDomainEvent : DomainEventBase
{
    public BookId BookId { get; private set; }
    public PatronId PatronId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public HoldId HoldId { get; set; }

    public HoldCanceledDomainEvent(BookId bookId, PatronId patronId, LibraryBranchId libraryBranchId, HoldId holdId)
    {
        BookId = bookId;
        PatronId = patronId;
        LibraryBranchId = libraryBranchId;
        HoldId = holdId;
    }
}