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
        
        hold.Loan();

        var holdCheckedOutDomainEvent = AssertDomainEventPublished<HoldLoanedDomainEvent>(hold);
        Assert.That(holdCheckedOutDomainEvent.HoldId, Is.EqualTo(hold.Id));
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }
    
    [Test]
    public void Loan_Fails_WhenHoldGranted()
    {
        var hold = CreateGrantedHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldGrantedRule>(() => hold.Loan());
        
        AssertHoldStatusGranted(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Loan_Fails_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldPendingRule>(() => hold.Loan());
        AssertHoldStatusPending(hold);
        AssertHoldActive(hold);
    }

    [Test]
    public void Loan_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldRejectedRule>(() => hold.Loan());
        AssertHoldStatusRejected(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Loan_Fails_WhenHoldCancelled()
    {
        var hold = CreateCancelledHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldCancelledRule>(() => hold.Loan());
        AssertHoldStatusCancelled(hold);
        AssertHoldNotActive(hold);
    }

    [Test]
    public void Loan_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldLoanedRule>(() => hold.Loan());
        AssertHoldStatusLoaned(hold);
        AssertHoldNotActive(hold);
    }
}