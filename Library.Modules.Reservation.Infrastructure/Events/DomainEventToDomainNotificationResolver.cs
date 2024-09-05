using Autofac;
using Autofac.Core;
using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure.Configuration;

namespace Library.Modules.Reservation.Infrastructure.Events;

internal class DomainEventToDomainNotificationResolver : IDomainEventToDomainNotificationResolver
{
    public IDomainNotification<IDomainEvent>? ResolveOptional(IDomainEvent domainEvent)
    {
        var scope = ReservationCompositionRoot.BeginLifetimeScope();
        var domainNotificationType = typeof(IDomainNotification<>);
        var domainNotificationGenericType = domainNotificationType.MakeGenericType(domainEvent.GetType());
        var domainNotification = scope.ResolveOptional(domainNotificationGenericType, new List<Parameter>
        {
            new NamedParameter("domainEvent", domainEvent),
            new NamedParameter("id", domainEvent.Id)
        });

        return domainNotification as IDomainNotification<IDomainEvent>;
    }
}