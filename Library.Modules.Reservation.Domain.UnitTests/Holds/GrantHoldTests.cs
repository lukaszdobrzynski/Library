﻿using Library.Modules.Reservation.Domain.Holds.Events;
using Library.Modules.Reservation.Domain.Holds.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class GrantHoldTests : HoldTestBase
{
    [Test]
    public void Grant_Succeeds_WhenHoldPending()
    {
        var hold = CreatePendingHold();
        
        hold.ApplyLibraryGrantDecision();

        var domainEvent = AssertDomainEventPublished<GrantHoldLibraryDecisionAppliedDomainEvent>(hold);
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
    }

    [Test]
    public void Grant_Fails_WhenHoldRejected()
    {
        var hold = CreateRejectedHold();
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldRejectedRule>(() => hold.ApplyLibraryGrantDecision());
    }

    [Test]
    public void Grant_Fails_WhenHoldLoaned()
    {
        var hold = CreateLoanedHold();
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldLoanedRule>(() => hold.ApplyLibraryGrantDecision());
    }

    [Test]
    public void Grant_Fails_WhenHoldCancelled()
    {
        var hold = CreateCancelledHold();

        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldCancelledRule>(() => hold.ApplyLibraryGrantDecision());
    }

    [Test]
    public void Grant_Fails_WhenHoldReadyToPick()
    {
        var hold = CreateHoldReadyToPick();
        
        AssertBusinessRuleBroken<CannotGrantHoldWhenHoldReadyToPickRule>(() => hold.ApplyLibraryGrantDecision());
    }
}