using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds.Events;

public class CancelHoldDecisionAppliedDomainEvent(HoldId holdId, BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId) : DomainEventBase
{
    public HoldId HoldId { get; } = holdId;
    public BookId BookId { get; set; } = bookId;
    public LibraryBranchId LibraryBranchId { get; set; } = libraryBranchId;
    public PatronId PatronId { get; set; } = patronId;
}