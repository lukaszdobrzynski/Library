using Autofac;
using Autofac.Core;
using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Infrastructure.Events;

public class DomainEventToDomainEventNotificationResolver : IDomainEventToDomainEventNotificationResolver
{
    private readonly ILifetimeScope _scope;
    
    public DomainEventToDomainEventNotificationResolver(ILifetimeScope scope)
    {
        _scope = scope;
    }

    public IDomainEventNotification<IDomainEvent>? ResolveOptional(IDomainEvent domainEvent)
    {
        var domainEventNotificationType = typeof(IDomainEventNotification<>);
        var domainEventNotificationGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());
        var domainNotification = _scope.ResolveOptional(domainEventNotificationGenericType, new List<Parameter>
        {
            new NamedParameter("domainEvent", domainEvent),
            new NamedParameter("id", domainEvent.Id)
        });

        return domainNotification as IDomainEventNotification<IDomainEvent>;
    }
}