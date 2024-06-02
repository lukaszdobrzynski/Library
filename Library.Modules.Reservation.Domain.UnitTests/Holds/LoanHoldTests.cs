using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class LoanHoldTests : HoldTestBase
{
    [Test]
    public void Succeeds_WhenHold_IsReadyToPick()
    {
        var hold = CreateReadyToPickHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        Act(hold);

        AssertDomainEventPublished<LoanHoldDecisionAppliedDomainEvent>(hold);
        
        AssertHoldInActive(hold);
    }
    
    [Test]
    public void Fails_WhenHold_IsGranted()
    {
        var hold = CreateGrantedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldGrantedRule>(() => Act(hold));
        
        AssertHoldActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsCancelled()
    {
        var hold = CreateCancelledHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldCancelledRule>(() => Act(hold));
        
        AssertHoldInActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsRejected()
    {
        var hold = CreateRejectedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldRejectedRule>(() => Act(hold));
        
        AssertHoldInActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsPending()
    {
        var hold = CreatePendingHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotLoanHoldWhenHoldPendingRule>(() => Act(hold));
        
        AssertHoldActive(hold);
    }

    private void Act(Hold hold)
    {
        hold.ApplyLoanDecision();
    }
}