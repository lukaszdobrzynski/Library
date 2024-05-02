using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class HoldTestBase : TestBase
{
    protected Hold CreatePendingHold() => Hold.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid()));

    protected Hold CreateGrantedHold()
    {
        var hold = CreatePendingHold();
        hold.Grant();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateLoanedHold()
    {
        var hold = CreatePendingHold();
        hold.Grant();
        hold.TagReadyToPick();
        hold.Loan();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateRejectedHold()
    {
        var hold = CreatePendingHold();
        hold.Reject();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateCancelledHold()
    {
        var hold = CreatePendingHold();
        hold.Grant();
        hold.CancelByLibrary();
        hold.ClearDomainEvents();
        return hold;
    }

    protected Hold CreateHoldReadyToPick()
    {
        var hold = CreatePendingHold();
        hold.Grant();
        hold.TagReadyToPick();
        hold.ClearDomainEvents();
        return hold;
    }
    
    protected void AssertHoldActive(Hold hold) => Assert.That(hold.IsActive, Is.True);
    protected void AssertHoldNotActive(Hold hold) => Assert.That(hold.IsActive, Is.False);
    protected void AssertHoldStatusPending(Hold hold) => Assert.That(string.Equals(hold.Status, HoldStatus.Pending), Is.True);
    protected void AssertHoldStatusGranted(Hold hold) => Assert.That(string.Equals(hold.Status, HoldStatus.Granted), Is.True);
    protected void AssertHoldStatusRejected(Hold hold) => Assert.That(string.Equals(hold.Status, HoldStatus.Rejected), Is.True);
    protected void AssertHoldStatusCancelled(Hold hold) => Assert.That(string.Equals(hold.Status, HoldStatus.Cancelled), Is.True);
    protected void AssertHoldStatusLoaned(Hold hold) => Assert.That(string.Equals(hold.Status, HoldStatus.Loaned), Is.True);
    protected void AssertHoldStatusReadyToPick(Hold hold) => Assert.That(string.Equals(hold.Status, HoldStatus.ReadyToPick), Is.True);
}