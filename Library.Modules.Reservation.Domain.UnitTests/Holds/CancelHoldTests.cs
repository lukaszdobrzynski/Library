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
    }

    [Test]
    public void CancelByLibrary_Succeeds_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        hold.ApplyLibraryCancelDecision();
        
        var domainEvent = AssertDomainEventPublished<CancelHoldLibraryDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
    }

    [Test]
    public void CancelByLibrary_Succeeds_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        hold.ApplyLibraryCancelDecision();
        
        var domainEvent = AssertDomainEventPublished<CancelHoldLibraryDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
    }
    
    [Test]
    public void CancelByPatron_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldPendingRule>(() => hold.ApplyPatronCancelDecision());
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldPendingRule>(() => hold.ApplyLibraryCancelDecision());
    }

    [Test]
    public void CancelByPatron_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldRejectedRule>(() => hold.ApplyPatronCancelDecision());
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldRejectedRule>(() => hold.ApplyLibraryCancelDecision());
    }
    
    [Test]
    public void CancelByPatron_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldLoanedRule>(() => hold.ApplyPatronCancelDecision());
    }
    
    [Test]
    public void CancelByLibrary_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldLoanedRule>(() => hold.ApplyLibraryCancelDecision());
    }

    [Test]
    public void CancelByPatron_Fails_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        AssertBusinessRuleBroken<PatronCannotCancelHoldWhenHoldReadyToPickRule>(() => hold.ApplyPatronCancelDecision());
    }
}