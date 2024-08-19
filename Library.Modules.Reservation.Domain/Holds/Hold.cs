using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public class Hold : AggregateRootBase
{
    public HoldId Id { get; private set; }
    public BookId BookId { get; private set; }
    public PatronId PatronId { get; private set; }
    public LibraryBranchId LibraryBranchId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? Till { get; private set; }
    public HoldStatus Status { get; private set; }
    public bool IsActive { get; set; }
    public HoldRequestId HoldRequestId { get; set; }

    private Hold()
    {
        // EF only
    }

    private Hold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, DateTime? till, HoldRequestId holdRequestId)
    {
        Id = new HoldId(Guid.NewGuid());
        BookId = bookId;
        LibraryBranchId = libraryBranchId;
        PatronId = patronId;
        Till = till;
        CreatedAt = DateTime.UtcNow;
        Status = HoldStatus.Pending;
        IsActive = IsHoldActive();
        HoldRequestId = holdRequestId;
        
        AddDomainEvent(new HoldCreatedDomainEvent(Id));
        IncreaseVersion();
    }

    public static Hold Create(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId, DateTime? till, HoldRequestId holdRequestId) =>
        new (bookId, libraryBranchId, patronId, till, holdRequestId);

    public void ApplyRejectDecision()
    {
        CheckRule(new CannotRejectHoldWhenHoldGrantedRule(Status));
        CheckRule(new CannotRejectHoldWhenHoldLoanedRule(Status));
        CheckRule(new CannotRejectHoldWhenHoldCancelledRule(Status));
        CheckRule(new CannotRejectHoldWhenHoldReadyToPickRule(Status));

        Status = HoldStatus.Rejected;
        IsActive = IsHoldActive();
        
        AddDomainEvent(new RejectHoldDecisionAppliedDomainEvent(Id, BookId));
        IncreaseVersion();
    }

    public void ApplyGrantDecision()
    {
        CheckRule(new CannotGrantHoldWhenHoldCancelledRule(Status));
        CheckRule(new CannotGrantHoldWhenHoldLoanedRule(Status));
        CheckRule(new CannotGrantHoldWhenHoldRejectedRule(Status));
        CheckRule(new CannotGrantHoldWhenHoldReadyToPickRule(Status));
        
        Status = HoldStatus.Granted;
        IsActive = IsHoldActive();
        
        AddDomainEvent(new GrantHoldDecisionAppliedDomainEvent(Id, BookId));
        IncreaseVersion();
    }

    public void ApplyLoanDecision()
    {
        CheckRule(new CannotLoanHoldWhenHoldGrantedRule(Status));
        CheckRule(new CannotLoanHoldWhenHoldCancelledRule(Status));
        CheckRule(new CannotLoanHoldWhenHoldRejectedRule(Status));
        CheckRule(new CannotLoanHoldWhenHoldPendingRule(Status));
        
        Status = HoldStatus.Loaned;
        IsActive = IsHoldActive();

        AddDomainEvent(new LoanHoldDecisionAppliedDomainEvent(Id, BookId));
        IncreaseVersion();
    }

    public void ApplyCancelDecision()
    {
        CheckRule(new CannotCancelHoldWhenHoldPendingRule(Status));
        CheckRule(new CannotCancelHoldWhenHoldRejectedRule(Status));
        CheckRule(new CannotCancelHoldWhenHoldLoanedRule(Status));
        
        Status = HoldStatus.Cancelled;
        IsActive = IsHoldActive();
        
        AddDomainEvent(new CancelHoldDecisionAppliedDomainEvent(Id, BookId, LibraryBranchId, PatronId));
        IncreaseVersion();
    }

    public void ApplyReadyToPickDecision()
    {
        CheckRule(new CannotTagHoldReadyToPickWhenHoldCancelledRule(Status));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldPendingRule(Status));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldRejectedRule(Status));
        CheckRule(new CannotTagHoldReadyToPickWhenHoldLoanedRule(Status));
        
        Status = HoldStatus.ReadyToPick;
        IsActive = IsHoldActive();
        
        AddDomainEvent(new CancelHoldDecisionAppliedDomainEvent(Id, BookId, LibraryBranchId, PatronId));
        IncreaseVersion();
    }
    
    private bool IsHoldActive()
    {
        return Status == HoldStatus.Pending || 
               Status == HoldStatus.Granted || 
               Status == HoldStatus.ReadyToPick;
    }
}