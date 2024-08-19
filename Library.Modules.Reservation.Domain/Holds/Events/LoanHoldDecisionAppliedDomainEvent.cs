using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Holds.Events;

public class LoanHoldDecisionAppliedDomainEvent(HoldId holdId, BookId bookId) : DomainEventBase
{
    public HoldId HoldId { get; } = holdId;
    public BookId BookId { get; set; } = bookId;
}