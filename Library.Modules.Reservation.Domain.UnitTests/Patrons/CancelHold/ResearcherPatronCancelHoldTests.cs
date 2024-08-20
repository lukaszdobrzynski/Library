using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Holds.Rules;
using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Domain.Patrons.Events;
using Library.Modules.Reservation.Domain.Patrons.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Patrons.CancelHold;

public class ResearcherPatronCancelHoldTests : TestBase
{
    [Test]
    public void Succeeds_WhenPatronOwnsHold()
    {
        var patron = Patron.CreateResearcher(Guid.NewGuid());

        var holdToCancel = HoldToCancel.Create(patron.Id, SomeHoldId1, SomeBookId1, SomeLibraryBranchId1, HoldStatus.Granted);
        
        patron.CancelHold(holdToCancel);

        var domainEvent = AssertDomainEventPublished<HoldCanceledDomainEvent>(patron);
        Assert.That(domainEvent, Is.Not.Null);
        Assert.That(domainEvent.PatronId, Is.EqualTo(holdToCancel.OwningPatronId));
        Assert.That(domainEvent.BookId, Is.EqualTo(holdToCancel.BookId));
        Assert.That(domainEvent.HoldId, Is.EqualTo(holdToCancel.HoldId));
        Assert.That(domainEvent.LibraryBranchId, Is.EqualTo(holdToCancel.LibraryBranchId));
    }
    
    [Test]
    public void Fails_WhenPatronDoesNotOwnHold()
    {
        var patron = Patron.CreateResearcher(Guid.NewGuid());

        var holdToCancel = HoldToCancel.Create(SomePatronId1, SomeHoldId1, SomeBookId1, SomeLibraryBranchId1, HoldStatus.Granted);
        
        Assert.That(patron.Id, Is.Not.EqualTo(holdToCancel.OwningPatronId));
        
        AssertBusinessRuleBroken<PatronCannotCancelHoldOwnedByAnotherPatronRule>(() => patron.CancelHold(holdToCancel));
    }

    [Test]
    public void Fails_WhenHoldStatus_ReadyToPick()
    {
        const string holdStatus = HoldStatus.ReadyToPick;
        var patron = Patron.CreateResearcher(Guid.NewGuid());

        var holdToCancel = HoldToCancel.Create(patron.Id, SomeHoldId1, SomeBookId1, SomeLibraryBranchId1,
            holdStatus);
        
        AssertBusinessRuleBroken<PatronCannotCancelHoldWhenHoldReadyToPickRule>(() => patron.CancelHold(holdToCancel));
    }
}