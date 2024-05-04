using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds.Rules;

namespace Library.Modules.Reservation.Domain.Holds;

public class Hold : Entity, IAggregateRoot
{
    public HoldId Id { get; private set; }
    public BookId BookId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public PatronHoldDecision PatronHoldDecision { get; private set; }
    public LibraryHoldDecision LibraryHoldDecision { get; private set; }
    public HoldStatus Status => HoldStatus.From(PatronHoldDecision.DecisionStatus, LibraryHoldDecision.DecisionStatus);

    public bool IsActive =>
        Status == HoldStatus.Pending || 
        Status == HoldStatus.Granted || 
        Status == HoldStatus.ReadyToPick;

    private Hold(BookId bookId, LibraryBranchId libraryBranchId)
    {
        Id = new HoldId(Guid.NewGuid());
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronHoldDecision = PatronHoldDecision.NoDecision(Id);
        LibraryHoldDecision = LibraryHoldDecision.NoDecision(Id);
    }

    public static Hold Create(BookId bookId, LibraryBranchId libraryBranchId) =>
        new (bookId, libraryBranchId);

    public void ApplyLibraryRejectDecision()
    {
        CheckRule(new CannotRejectHoldWhenHoldGrantedRule(Status));
        CheckRule(new CannotRejectHoldWhenHoldLoanedRule(Status));
        CheckRule(new CannotRejectHoldWhenHoldCancelledRule(Status));
        CheckRule(new CannotRejectHoldWhenHoldReadyToPickRule(Status));
        
        LibraryHoldDecision.Reject();
    }

    public void ApplyLibraryGrantDecision()
    {
        CheckRule(new CannotGrantHoldWhenHoldCancelledRule(Status));
        CheckRule(new CannotGrantHoldWhenHoldLoanedRule(Status));
        CheckRule(new CannotGrantHoldWhenHoldRejectedRule(Status));
        CheckRule(new CannotGrantHoldWhenHoldReadyToPickRule(Status));
        
        LibraryHoldDecision.Grant();
    }

    public void ApplyLibraryLoanDecision()
    {
        CheckRule(new CannotLoanHoldWhenHoldGrantedRule(Status));
        CheckRule(new CannotLoanHoldWhenHoldCancelledRule(Status));
        CheckRule(new CannotLoanHoldWhenHoldRejectedRule(Status));
        CheckRule(new CannotLoanHoldWhenHoldPendingRule(Status));
        
        LibraryHoldDecision.Loan();
    }

    public void ApplyLibraryCancelDecision()
    {
        CheckRule(new CannotCancelHoldWhenHoldPendingRule(Status));
        CheckRule(new CannotCancelHoldWhenHoldRejectedRule(Status));
        CheckRule(new CannotCancelHoldWhenHoldLoanedRule(Status));

        LibraryHoldDecision.Cancel();
    }

    public void ApplyPatronCancelDecision()
    {
        CheckRule(new CannotCancelHoldWhenHoldPendingRule(Status));
        CheckRule(new CannotCancelHoldWhenHoldRejectedRule(Status));
        CheckRule(new CannotCancelHoldWhenHoldLoanedRule(Status));
        CheckRule(new PatronCannotCancelHoldWhenHoldReadyToPickRule(Status));
        
        PatronHoldDecision.Cancel();
    }

    public void ApplyLibraryReadyToPickDecision()
    {
        CheckRule(new CannotTagHoldReadyToPickWhenHoldCancelledRule(Status));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldPendingRule(Status));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldRejectedRule(Status));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldLoanedRule(Status));
        
        LibraryHoldDecision.TagReadyToPick();
    }
}