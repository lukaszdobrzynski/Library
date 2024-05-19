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

        var domainEvent = AssertDomainEventPublished<LoanHoldLibraryDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
    }
    
    [Test]
    public void Loan_Fails_WhenHoldGranted_AndHoldDecisionParty_IsLibrary()
    {
        var hold = CreateGrantedHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldGrantedRule>(() => hold.ApplyLibraryLoanDecision());
    }

    [Test]
    public void Loan_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldPendingRule>(() => hold.ApplyLibraryLoanDecision());
    }

    [Test]
    public void Loan_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldRejectedRule>(() => hold.ApplyLibraryLoanDecision());
    }

    [Test]
    public void Loan_Fails_WhenHoldCancelled()
    {
        var hold = CreateCancelledHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldCancelledRule>(() => hold.ApplyLibraryLoanDecision());
    }
}