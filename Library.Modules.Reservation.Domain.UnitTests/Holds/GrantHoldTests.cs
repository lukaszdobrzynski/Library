using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class GrantHoldTests : HoldTestBase
{
    [Test]
    public void Grant_Succeeds_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        hold.Grant();

        var holdGrantedDomainEvent = AssertDomainEventPublished<HoldGrantedDomainEvent>(hold);
        Assert.That(holdGrantedDomainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusGranted(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Grant_Fails_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldGrantedRule>(() => hold.Grant());
        AssertHoldStatusGranted(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Grant_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldRejectedRule>(() => hold.Grant());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Grant_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldLoanedRule>(() => hold.Grant());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Grant_Fails_WhenHoldCancelled()
    {
        var hold = CreateCancelledHold();

        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldCancelledRule>(() => hold.Grant());
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Grant_Fails_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldReadyToPickRule>(() => hold.Grant());
        AssertHoldStatusReadyToPick(hold);
        AssertHoldActive(hold);
    }
}