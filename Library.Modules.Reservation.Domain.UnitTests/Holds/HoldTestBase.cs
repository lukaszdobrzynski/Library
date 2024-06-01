using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class HoldTestBase : TestBase
{
    protected Hold CreateGrantedHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        var hold = CreatePendingHold(bookId, libraryBranchId, patronId);
        hold.ApplyGrantDecision();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreatePendingHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        var hold = Hold.Create(bookId, libraryBranchId, patronId, null);
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateReadyToPickHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        var hold = CreateGrantedHold(bookId, libraryBranchId, patronId);
        hold.ApplyReadyToPickDecision();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateCancelledHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        var hold = CreateGrantedHold(bookId, libraryBranchId, patronId);
        hold.ApplyPatronCancelDecision();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateLoanedHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        var hold = CreateGrantedHold(bookId, libraryBranchId, patronId);
        hold.ApplyReadyToPickDecision();
        hold.ApplyLoanDecision();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateRejectedHold(BookId bookId, LibraryBranchId libraryBranchId, PatronId patronId)
    {
        var hold = CreatePendingHold(bookId, libraryBranchId, patronId);
        hold.ApplyRejectDecision();
        hold.ClearDomainEvents();
        return hold;
    }

    protected void AssertHoldActive(Hold hold)
    {
        Assert.That(hold.IsActive, Is.True);
    }
    
    protected void AssertHoldInActive(Hold hold)
    {
        Assert.That(hold.IsActive, Is.False);
    }
}