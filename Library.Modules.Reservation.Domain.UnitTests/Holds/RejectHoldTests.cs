using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class RejectHoldTests : HoldTestBase
{
    [Test]
    public void Reject_Succeeds_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        hold.ApplyLibraryRejectDecision();

        var domainEvent = AssertDomainEventPublished<LibraryRejectHoldDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void Reject_Fails_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldGrantedRule>(() => hold.ApplyLibraryRejectDecision());
        AssertHoldStatusGranted(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Reject_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldLoanedRule>(() => hold.ApplyLibraryRejectDecision());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }
}