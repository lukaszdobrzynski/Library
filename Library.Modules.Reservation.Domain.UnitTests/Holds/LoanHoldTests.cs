using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class LoanHoldTests : HoldTestBase
{
    [Test]
    public void Loan_Succeeds_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        hold.ApplyLibraryLoanDecision();

        var holdCheckedOutDomainEvent = AssertDomainEventPublished<HoldLoanedDomainEvent>(hold);
        Assert.That(holdCheckedOutDomainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void Loan_Fails_WhenHoldGranted_AndHoldDecisionParty_IsLibrary()
    {
        var hold = CreateGrantedHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldGrantedRule>(() => hold.ApplyLibraryLoanDecision());
        
        AssertHoldStatusGranted(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Loan_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldPendingRule>(() => hold.ApplyLibraryLoanDecision());
        AssertHoldStatusPending(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Loan_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldRejectedRule>(() => hold.ApplyLibraryLoanDecision());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Loan_Fails_WhenHoldCancelled()
    {
        var hold = CreateCancelledHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldCancelledRule>(() => hold.ApplyLibraryLoanDecision());
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }
}