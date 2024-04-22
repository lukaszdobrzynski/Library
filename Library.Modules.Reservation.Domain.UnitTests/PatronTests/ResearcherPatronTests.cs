using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Domain.Patrons.Events;
using Library.Modules.Reservation.Domain.Patrons.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.PatronTests;

public class ResearcherPatronTests : PatronTestBase
{
    [Test]
    public void Create_Succeeds()
    {
        var patron = CreateResearcherPatron();

        var researcherPatronCreated = AssertDomainEventPublished<ResearcherPatronCreatedDomainEvent>(patron);
        
        Assert.That(researcherPatronCreated.PatronId, Is.EqualTo(patron.Id));
    }

    [Test]
    public void PlaceOnHold_Succeeds_WhenNoHolds()
    {
        var patron = CreateResearcherPatron();
        var book = CreateBookToHold_Circulating();
        
        patron.PlaceOnHold(book, WithEmptyActiveHolds, WithEmptyOverdueCheckouts);

        var bookPlacedOnHold = AssertDomainEventPublished<BookPlacedOnHoldDomainEvent>(patron);
        Assert.That(bookPlacedOnHold.BookId, Is.EqualTo(book.BookId));
    }

    [Test]
    public void PlaceOnHold_Fails_WhenHoldAlreadyRegistered()
    {
        var patron = CreateResearcherPatron();
        var book = CreateBookToHold_Circulating();
        
        AssertBusinessRuleBroken<PatronCannotPlaceHoldOnAlreadyRegisteredHoldRule>(
            () => patron.PlaceOnHold(book, WithActiveHolds(ActiveHold.Create(book.BookId)), WithEmptyOverdueCheckouts));
    }

    [Test]
    public void PlaceOnHold_Succeeds_When_BookCategory_IsRestricted()
    {
        var patron = CreateResearcherPatron();
        var book = CreateBookToHold_Restricted();

        patron.PlaceOnHold(book, WithEmptyActiveHolds, WithEmptyOverdueCheckouts);
        
        var bookPlacedOnHold = AssertDomainEventPublished<BookPlacedOnHoldDomainEvent>(patron);
        Assert.That(bookPlacedOnHold.BookId, Is.EqualTo(book.BookId));
    }

    [Test]
    public void PlaceOnHold_Fails_When_MaxOverdueCheckoutsLimitExceeded()
    {
        var patron = CreateResearcherPatron();
        var book = CreateBookToHold_Circulating();
        
        var overdueCheckouts = CreateMany(
            PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule.MaxAllowedOverdueCheckouts,
            () => new OverdueCheckout(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid())));

        AssertBusinessRuleBroken<PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule>(
            () => patron.PlaceOnHold(book, WithEmptyActiveHolds, overdueCheckouts));
    }

    [Test]
    public void CancelHold_Fails_WhenHoldNotOwnedByCancellingPatron()
    {
        var owningPatron = CreateResearcherPatron();
        var notOwningPatronId = new PatronId(Guid.NewGuid());
        
        var bookOnHoldToCancel = BookOnHold.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid()), notOwningPatronId);
        
        Assert.That(owningPatron.Id, Is.Not.EqualTo(notOwningPatronId));
        
        AssertBusinessRuleBroken<PatronCannotCancelHoldOwnedByAnotherPatronRule>(
            () => owningPatron.CancelHold(bookOnHoldToCancel, WithActiveHolds(ActiveHold.Create(bookOnHoldToCancel.BookId))));
    }

    [Test]
    public void CancelHold_Fails_WhenHoldNotExists()
    {
        var patron = CreateResearcherPatron();

        var notExistingHold = BookOnHold.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid()),
            patron.Id);
        
        AssertBusinessRuleBroken<PatronCannotCancelUnregisteredHoldRule>(() => patron.CancelHold(notExistingHold, 
            WithActiveHolds(ActiveHold.Create(new BookId(Guid.NewGuid())))));
    }

    [Test]
    public void CancelHold_Succeeds()
    {
        var patron = CreateResearcherPatron();

        var holdToCancel = BookOnHold.Create(new BookId(Guid.NewGuid()), new LibraryBranchId(Guid.NewGuid()), patron.Id);
        patron.CancelHold(holdToCancel, [ActiveHold.Create(holdToCancel.BookId)]);

        AssertDomainEventPublished<BookHoldCanceledDomainEvent>(patron);
    }

    private static ResearcherPatron CreateResearcherPatron() => ResearcherPatron.Create(Guid.NewGuid());
}