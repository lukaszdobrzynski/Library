using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class CancelHoldTests : HoldTestBase
{
    [Test]
    public void CancelByPatron_Succeeds_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        hold.CancelByPatron();
        
        var holdGrantedDomainEvent = AssertDomainEventPublished<HoldCancelledByPatronDomainEvent>(hold);
        Assert.That(holdGrantedDomainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void CancelByLibrary_Succeeds_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        hold.CancelByLibrary();
        
        var holdGrantedDomainEvent = AssertDomainEventPublished<HoldCancelledByLibraryDomainEvent>(hold);
        Assert.That(holdGrantedDomainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void CancelByLibrary_Succeeds_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        hold.CancelByLibrary();
        
        var holdGrantedDomainEvent = AssertDomainEventPublished<HoldCancelledByLibraryDomainEvent>(hold);
        Assert.That(holdGrantedDomainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByPatron_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldPendingRule>(() => hold.CancelByPatron());
        AssertHoldStatusPending(hold);
        AssertHoldActive(hold);
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldPendingRule>(() => hold.CancelByPatron());
        AssertHoldStatusPending(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void CancelByPatron_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldRejectedRule>(() => hold.CancelByPatron());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldRejectedRule>(() => hold.CancelByLibrary());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByPatron_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldLoanedRule>(() => hold.CancelByPatron());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldLoanedRule>(() => hold.CancelByLibrary());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void CancelByPatron_Fails_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldReadyToPickRule>(() => hold.CancelByPatron());
        AssertHoldStatusReadyToPick(hold);
        AssertHoldActive(hold);
    }
}