using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class GrantHoldTests : HoldTestBase
{
    [Test]
    public void Succeeds_WhenHold_IsPending()
    {
        var hold = CreatePendingHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        Act(hold);

        AssertDomainEventPublished<HoldGrantedDomainEvent>(hold);
        
        AssertHoldActive(hold);
    }
    
    [Test]
    public void Fails_WhenHold_IsCancelled()
    {
        var hold = CreateCancelledHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldCancelledRule>(() => Act(hold));
        
        AssertHoldInActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsLoaned()
    {
        var hold = CreateLoanedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldLoanedRule>(() => Act(hold));
        
        AssertHoldInActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsRejected()
    {
        var hold = CreateRejectedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldRejectedRule>(() => Act(hold));
        
        AssertHoldInActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsReadyToPick()
    {
        var hold = CreateReadyToPickHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldReadyToPickRule>(() => Act(hold));
        
        AssertHoldActive(hold);
    }

    private void Act(Hold hold)
    {
        hold.ApplyGrantDecision();
    }
}