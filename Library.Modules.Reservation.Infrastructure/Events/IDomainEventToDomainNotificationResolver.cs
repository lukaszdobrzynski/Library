using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Infrastructure.Events;

internal interface IDomainEventToDomainNotificationResolver
{
    IDomainNotification<IDomainEvent>? ResolveOptional(IDomainEvent domainEvent);
}