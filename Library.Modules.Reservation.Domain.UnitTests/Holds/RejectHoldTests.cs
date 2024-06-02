using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class RejectHoldTests : HoldTestBase
{
    [Test]
    public void Succeeds_WhenHold_IsPending()
    {
        var hold = CreatePendingHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        Act(hold);

        AssertDomainEventPublished<RejectHoldDecisionAppliedDomainEvent>(hold);
    }
    
    [Test]
    public void Fails_WhenHold_IsGranted()
    {
        var hold = CreateGrantedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldGrantedRule>(() => Act(hold));
    }

    [Test]
    public void Fails_WhenHold_IsLoaned()
    {
        var hold = CreateLoanedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldLoanedRule>(() => Act(hold));
    }

    [Test]
    public void Fails_WhenHold_IsCancelled()
    {
        var hold = CreateCancelledHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldCancelledRule>(() => Act(hold));
    }

    [Test]
    public void Fails_WhenHold_ReadyToPick()
    {
        var hold = CreateReadyToPickHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotRejectHoldWhenHoldReadyToPickRule>(() => Act(hold));
    }

    private void Act(Hold hold)
    {
        hold.ApplyRejectDecision();
    }
}