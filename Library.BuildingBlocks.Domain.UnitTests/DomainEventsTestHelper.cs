using System.Collections;
using System.Reflection;

namespace Library.BuildingBlocks.Domain.UnitTests;

public class DomainEventsTestHelper
{
    public static List<IDomainEvent> GetDomainEvents(Entity aggregate)
    {
        var domainEvents = new List<IDomainEvent>();

        if (aggregate.DomainEvents != null)
        {
            domainEvents.AddRange(aggregate.DomainEvents);
        }

        var fields = aggregate.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Concat(aggregate.GetType().BaseType
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            .ToArray();

        foreach (var field in fields)
        {
            var isEntity = typeof(Entity).IsAssignableFrom(field.FieldType);

            if (isEntity)
            {
                var entity = field.GetValue(aggregate) as Entity;
                if (entity != null)
                {
                    domainEvents.AddRange(GetDomainEvents(entity));    
                }
            }

            if (field.FieldType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
            {
                if (field.GetValue(aggregate) is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item is Entity entityItem)
                        {
                            domainEvents.AddRange(GetDomainEvents(entityItem));
                        }
                    }
                }
            }
        }

        return domainEvents;
    }
}