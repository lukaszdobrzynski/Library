using Library.BuildingBlocks.Application.Events;
using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Infrastructure;

public interface IDomainEventToDomainEventNotificationResolver
{
    IDomainEventNotification<IDomainEvent>? ResolveOptional(IDomainEvent domainEvent);
}