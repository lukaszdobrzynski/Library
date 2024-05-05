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
        
        hold.ApplyPatronCancelDecision();
        
        var domainEvent = AssertDomainEventPublished<CancelHoldPatronDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void CancelByLibrary_Succeeds_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        hold.ApplyLibraryCancelDecision();
        
        var domainEvent = AssertDomainEventPublished<CancelHoldLibraryDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void CancelByLibrary_Succeeds_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        hold.ApplyLibraryCancelDecision();
        
        var domainEvent = AssertDomainEventPublished<CancelHoldLibraryDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByPatron_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldPendingRule>(() => hold.ApplyPatronCancelDecision());
        AssertHoldStatusPending(hold);
        AssertHoldActive(hold);
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldPendingRule>(() => hold.ApplyLibraryCancelDecision());
        AssertHoldStatusPending(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void CancelByPatron_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldRejectedRule>(() => hold.ApplyPatronCancelDecision());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldRejectedRule>(() => hold.ApplyLibraryCancelDecision());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByPatron_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldLoanedRule>(() => hold.ApplyPatronCancelDecision());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldLoanedRule>(() => hold.ApplyLibraryCancelDecision());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void CancelByPatron_Fails_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        AssertBusinessRuleBroken<PatronCannotCancelHoldWhenHoldReadyToPickRule>(() => hold.ApplyPatronCancelDecision());
        AssertHoldStatusReadyToPick(hold);
        AssertHoldActive(hold);
    }
}