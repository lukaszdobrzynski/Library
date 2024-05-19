using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Events;

public class BookPlacedOnHoldDomainEvent(BookId bookId, PatronId patronId, LibraryBranchId libraryBranchId, HoldPeriod period)
    : DomainEventBase
{
    public BookId BookId { get; private set; } = bookId;
    public PatronId PatronId { get; private set; } = patronId;
    public LibraryBranchId LibraryBranchId { get; private set; } = libraryBranchId;
    public HoldPeriod Period { get; set; } = period;
}