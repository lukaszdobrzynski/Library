using Library.BuildingBlocks.Domain;
using Library.BuildingBlocks.Domain.UnitTests;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Domain.Patrons.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests;

public abstract class TestBase
{
    protected static T AssertDomainEventPublished<T>(Entity aggregate)
        where T : IDomainEvent
    {
        var domainEvent = DomainEventsTestHelper.GetDomainEvents(aggregate)
            .OfType<T>().SingleOrDefault();

        
        Assert.That(domainEvent, Is.Not.Null, $"{typeof(T).Name} event not published.");
        
        return domainEvent;
    }

    protected static void AssertBusinessRuleBroken<TRule>(TestDelegate testDelegate)
        where TRule : class, IBusinessRule
    {
        var message = $"Expected {typeof(TRule).Name} broken rule.";
        var businessRuleValidationException = Assert.Catch<BusinessRuleValidationException>(testDelegate, message);

        if (businessRuleValidationException != null)
        {
            Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
        }
    }

    public List<OverdueCheckout> EmptyOverdueCheckouts => [];
    protected List<OverdueCheckout> OverdueCheckoutsWithLimitExceeded =>
        Enumerable.Range(0, PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule.MaxAllowedOverdueCheckouts)
            .Select(_ =>
            {
                var bookId = new BookId(Guid.NewGuid());
                var libraryBranchId = SomeLibraryBranchId1;
                return OverdueCheckout.Create(bookId, libraryBranchId);
            }).ToList();
    
    public ActiveHolds EmptyActiveHolds => ActiveHolds.Create(0);
    protected BookId SomeBookId1 => new (Guid.Parse("97851c0d-698a-448c-af7e-1e148e716385"));
    protected LibraryBranchId SomeLibraryBranchId1 => new (Guid.Parse("ed5d4356-c986-4d66-9fc9-db613f104416"));

    protected PatronId SomePatronId1 = new (Guid.Parse("76eb84df-33ee-4db0-9854-1d18cc5124d3"));

    protected HoldId SomeHoldId1 = new (Guid.Parse("a9028244-d556-49db-a111-dab4f2c9cb20"));
    protected BookId SomeBookId2 => new (Guid.Parse("535b81b4-50c3-4045-9acb-2ba672016573"));
    protected LibraryBranchId SomeLibraryBranchId2 => new (Guid.Parse("0672baf7-a224-4544-b563-643c9711b2a8"));
    
    protected PatronId SomePatronId2 = new (Guid.Parse("3f8b6779-dd8e-4a86-979f-d53814d221a7"));

    protected HoldId SomeHoldId2 = new (Guid.Parse("2968cfc0-143e-44fa-9a34-cd985e9a2b2b"));
    protected BookId SomeBookId3 => new (Guid.Parse("17fa8c76-4030-447e-a0e9-0ab81affeef4"));
    protected LibraryBranchId SomeLibraryBranchId3 => new (Guid.Parse("f04393fb-8301-4778-84cb-f00ee9d08955"));
    
    protected PatronId SomePatronId3 = new (Guid.Parse("143b9cdd-a3af-4153-a4c2-81db7191e421"));

    protected HoldId SomeHoldId3 = new (Guid.Parse("1ee822dd-1634-4bc2-b493-645dc6682e00"));
}