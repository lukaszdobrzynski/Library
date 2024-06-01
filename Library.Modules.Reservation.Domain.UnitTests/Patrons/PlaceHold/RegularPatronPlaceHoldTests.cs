using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Domain.Patrons.Events;
using Library.Modules.Reservation.Domain.Patrons.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Patrons.PlaceHold;

public class RegularPatronPlaceHoldTests : TestBase
{
    [Test]
    public void Succeeds()
    {
        var patron = Patron.CreateRegular(Guid.NewGuid());

        var bookToHold = BookToHoldBuilder
            .InitCirculating(SomeBookId1, SomeLibraryBranchId1)
            .Build();
        
        patron.PlaceOnHold(bookToHold, EmptyOverdueCheckouts, EmptyActiveHolds);

        var domainEvent = AssertDomainEventPublished<BookPlacedOnHoldDomainEvent>(patron);
        
        Assert.That(domainEvent, Is.Not.Null);
        Assert.That(domainEvent.BookId, Is.EqualTo(SomeBookId1));
        Assert.That(domainEvent.LibraryBranchId, Is.EqualTo(SomeLibraryBranchId1));
        Assert.That(domainEvent.PatronId, Is.EqualTo(patron.Id));
        Assert.That(domainEvent.Till, Is.Not.Null);
        Assert.That(domainEvent.Till, Is.EqualTo(DateTime.Today.AddDays(7)));
    }
    
    [Test]
    public void Fails_WhenBookOnActiveHold()
    {
        var patron = Patron.CreateRegular(Guid.NewGuid());

        var bookToHold = BookToHoldBuilder
            .InitCirculating(SomeBookId1, SomeLibraryBranchId2)
            .OnActiveHold()
            .Build();
        
        AssertBusinessRuleBroken<PatronCannotPlaceHoldWhenBookOnActiveHoldRule>(
            () => patron.PlaceOnHold(bookToHold, EmptyOverdueCheckouts, EmptyActiveHolds));
    }

    [Test]
    public void Fails_WhenBookIsRestricted()
    {
        var patron = Patron.CreateRegular(Guid.NewGuid());

        var bookToHold = BookToHoldBuilder
            .InitRestricted(SomeBookId1, SomeLibraryBranchId2)
            .Build();
        
        AssertBusinessRuleBroken<RegularPatronCannotPlaceHoldOnRestrictedBookRule>(() => 
            patron.PlaceOnHold(bookToHold, EmptyOverdueCheckouts, EmptyActiveHolds));
    }

    [Test]
    public void Fails_WhenMaxHoldsLimitExceeded()
    {
        var patron = Patron.CreateRegular(Guid.NewGuid());

        var bookToHold = BookToHoldBuilder
            .InitCirculating(SomeBookId1, SomeLibraryBranchId2)
            .Build();

        var activeHolds = ActiveHolds.Create(RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule.MaxAllowedHolds);
        
        AssertBusinessRuleBroken<RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule>(() => 
            patron.PlaceOnHold(bookToHold, EmptyOverdueCheckouts, activeHolds));
    }

    [Test]
    public void Fails_WhenOverdueCheckoutsLimitExceeded()
    {
        var patron = Patron.CreateRegular(Guid.NewGuid());

        var bookToHold = BookToHoldBuilder
            .InitCirculating(SomeBookId1, SomeLibraryBranchId1)
            .Build();
        
        AssertBusinessRuleBroken<PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule>(() => 
            patron.PlaceOnHold(bookToHold, OverdueCheckoutsWithLimitExceeded, EmptyActiveHolds));
    }
}