using Library.BuildingBlocks.Application.Events;
using Library.BuildingBlocks.Domain;

namespace Library.BuildingBlocks.Infrastructure;

public interface IDomainEventToDomainEventNotificationResolver
{
    IDomainEventNotification<IDomainEvent>? ResolveOptional(IDomainEvent domainEvent);
}