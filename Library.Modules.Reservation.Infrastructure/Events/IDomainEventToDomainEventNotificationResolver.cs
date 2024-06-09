using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Infrastructure.Events;

public interface IDomainEventToDomainEventNotificationResolver
{
    IDomainEventNotification<IDomainEvent>? ResolveOptional(IDomainEvent domainEvent);
}