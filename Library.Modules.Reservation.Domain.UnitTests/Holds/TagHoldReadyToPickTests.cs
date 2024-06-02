using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class TagHoldReadyToPickTests : HoldTestBase
{
    [Test]
    public void Succeeds_WhenHold_IsGranted()
    {
        var hold = CreateGrantedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        Act(hold);

        AssertDomainEventPublished<CancelHoldDecisionAppliedDomainEvent>(hold);
    }
    
    [Test]
    public void Fails_WhenHold_IsCancelled()
    {
        var hold = CreateCancelledHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotTagHoldReadyToPickWhenHoldCancelledRule>(() => Act(hold));
    }

    [Test]
    public void Fails_WhenHold_IsPending()
    {
        var hold = CreatePendingHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotTagHoldReadyToPickWhenHoldPendingRule>(() => Act(hold));
    }

    [Test]
    public void Fails_WhenHold_IsRejected()
    {
        var hold = CreateRejectedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotTagHoldReadyToPickWhenHoldRejectedRule>(() => Act(hold));
    }

    [Test]
    public void Fails_WhenHold_IsLoaned()
    {
        var hold = CreateLoanedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotTagHoldReadyToPickWhenHoldLoanedRule>(() => Act(hold));
    }

    private void Act(Hold hold)
    {
        hold.ApplyReadyToPickDecision();
    }
}