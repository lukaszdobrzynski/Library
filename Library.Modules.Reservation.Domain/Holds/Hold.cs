using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public class Hold : Entity, IAggregateRoot
{
    public HoldId Id { get; private set; }
    public BookId BookId { get; private set; }
    public PatronId PatronId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public PatronHoldDecision PatronHoldDecision { get; private set; }
    public LibraryHoldDecision LibraryHoldDecision { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public HoldPeriod Period { get; private set; }

    private Hold()
    {
        // EF only
    }

    private Hold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, HoldPeriod period)
    {
        Id = new HoldId(Guid.NewGuid());
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronId = patronId;
        PatronHoldDecision = PatronHoldDecision.NoDecision(Id);
        LibraryHoldDecision = LibraryHoldDecision.NoDecision(Id);
        Period = period;
        CreatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new HoldCreatedDomainEvent(Id));
    }

    public static Hold Create(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, HoldPeriod period) =>
        new (bookId, libraryBranchId, patronId, period);

    public void ApplyLibraryRejectDecision()
    {
        CheckRule(new CannotRejectHoldWhenHoldGrantedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotRejectHoldWhenHoldLoanedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotRejectHoldWhenHoldCancelledRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotRejectHoldWhenHoldReadyToPickRule(PatronHoldDecision, LibraryHoldDecision));
        
        LibraryHoldDecision.Reject();
    }

    public void ApplyLibraryGrantDecision()
    {
        CheckRule(new CannotGrantHoldWhenHoldCancelledRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotGrantHoldWhenHoldLoanedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotGrantHoldWhenHoldRejectedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotGrantHoldWhenHoldReadyToPickRule(PatronHoldDecision, LibraryHoldDecision));
        
        LibraryHoldDecision.Grant();
    }

    public void ApplyLibraryLoanDecision()
    {
        CheckRule(new CannotLoanHoldWhenHoldGrantedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotLoanHoldWhenHoldCancelledRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotLoanHoldWhenHoldRejectedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotLoanHoldWhenHoldPendingRule(PatronHoldDecision, LibraryHoldDecision));
        
        LibraryHoldDecision.Loan();
    }

    public void ApplyLibraryCancelDecision()
    {
        CheckRule(new CannotCancelHoldWhenHoldPendingRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotCancelHoldWhenHoldRejectedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotCancelHoldWhenHoldLoanedRule(PatronHoldDecision, LibraryHoldDecision));

        LibraryHoldDecision.Cancel();
    }

    public void ApplyPatronCancelDecision()
    {
        CheckRule(new CannotCancelHoldWhenHoldPendingRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotCancelHoldWhenHoldRejectedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotCancelHoldWhenHoldLoanedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new PatronCannotCancelHoldWhenHoldReadyToPickRule(PatronHoldDecision, LibraryHoldDecision));
        
        PatronHoldDecision.Cancel();
    }

    public void ApplyLibraryReadyToPickDecision()
    {
        CheckRule(new CannotTagHoldReadyToPickWhenHoldCancelledRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldPendingRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldRejectedRule(PatronHoldDecision, LibraryHoldDecision));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldLoanedRule(PatronHoldDecision, LibraryHoldDecision));
        
        LibraryHoldDecision.TagReadyToPick();
    }
}