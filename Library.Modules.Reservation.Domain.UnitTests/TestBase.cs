using Library.BuildingBlocks.Domain;
using Library.BuildingBlocks.Domain.UnitTests;
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
}