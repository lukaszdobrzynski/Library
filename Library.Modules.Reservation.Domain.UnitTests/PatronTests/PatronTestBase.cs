using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.UnitTests.PatronTests;

public abstract class PatronTestBase : TestBase
{
    protected static List<ActiveHold> WithEmptyActiveHolds => [];
    protected static List<OverdueCheckout> WithEmptyOverdueCheckouts => [];
    protected static List<ActiveHold> WithActiveHolds(params ActiveHold[] holds) => [..holds];
    
    protected static BookToHold CreateBookToHold_Circulating()
    {
        return BookToHold.Create(
            new BookId(Guid.NewGuid()),
            new LibraryBranchId(Guid.NewGuid()),
            new PatronId(Guid.NewGuid()),
            BookCategory.Circulating);
    }

    protected static BookToHold CreateBookToHold_Restricted()
    {
        return BookToHold.Create(
            new BookId(Guid.NewGuid()),
            new LibraryBranchId(Guid.NewGuid()),
            new PatronId(Guid.NewGuid()),
            BookCategory.Restricted);
    }
    
    protected static List<T> CreateMany<T>(int count, Func<T> creator)
    {
        return Enumerable
            .Range(1, count)
            .Select(_ => creator())
            .ToList();
    }
}