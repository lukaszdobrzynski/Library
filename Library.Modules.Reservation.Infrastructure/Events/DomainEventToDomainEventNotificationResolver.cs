using Autofac;
using Autofac.Core;
using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure.Configuration;

namespace Library.Modules.Reservation.Infrastructure.Events;

public class DomainEventToDomainEventNotificationResolver : IDomainEventToDomainEventNotificationResolver
{
    public IDomainEventNotification<IDomainEvent>? ResolveOptional(IDomainEvent domainEvent)
    {
        var scope = ReservationCompositionRoot.BeginLifetimeScope();
        var domainEventNotificationType = typeof(IDomainEventNotification<>);
        var domainEventNotificationGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());
        var domainNotification = scope.ResolveOptional(domainEventNotificationGenericType, new List<Parameter>
        {
            new NamedParameter("domainEvent", domainEvent),
            new NamedParameter("id", domainEvent.Id)
        });

        return domainNotification as IDomainEventNotification<IDomainEvent>;
    }
}