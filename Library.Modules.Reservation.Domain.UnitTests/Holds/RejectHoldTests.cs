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
        
        hold.Reject();

        var holdGrantedDomainEvent = AssertDomainEventPublished<HoldRejectedDomainEvent>(hold);
        Assert.That(holdGrantedDomainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void Reject_Fails_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldGrantedRule>(() => hold.Reject());
        AssertHoldStatusGranted(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Reject_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldLoanedRule>(() => hold.Reject());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Reject_Fails_WhenHoldCancelled()
    {
        var hold = CreateCancelledHold();
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldCancelledRule>(() => hold.Reject());
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Reject_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldRejectedRule>(() => hold.Reject());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Reject_Fails_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldReadyToPickRule>(() => hold.Reject());
        AssertHoldStatusReadyToPick(hold);
        AssertHoldActive(hold);
    }
}