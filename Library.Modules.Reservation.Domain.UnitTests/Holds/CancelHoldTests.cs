﻿using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class CancelHoldTests : HoldTestBase
{
    [Test]
    public void Succeeds_WhenHold_IsGranted()
    {
        var hold = CreateGrantedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        Act(hold);

        AssertDomainEventPublished<CancelHoldDecisionAppliedDomainEvent>(hold);
        
        AssertHoldInActive(hold);
    }
    
    [Test]
    public void Fails_WhenHold_IsPending()
    {
        var hold = CreatePendingHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldPendingRule>(() => Act(hold));
        
        AssertHoldActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsRejected()
    {
        var hold = CreateRejectedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldRejectedRule>(() => Act(hold));
        
        AssertHoldInActive(hold);
    }

    [Test]
    public void Fails_WhenHold_IsLoaned()
    {
        var hold = CreateLoanedHold(SomeBookId1, SomeLibraryBranchId1, SomePatronId1);
        
        AssertBusinessRuleBroken<CannotCancelHoldWhenHoldLoanedRule>(() => Act(hold));
        
        AssertHoldInActive(hold);
    }

    private static void Act(Hold hold)
    {
        hold.ApplyCancelDecision();
    }
}