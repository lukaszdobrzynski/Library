using Library.BuildingBlocks.Domain;
using MediatR;

namespace Library.BuildingBlocks.Application.Events;

public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
 where TEventType : IDomainEvent
{
    TEventType DomainEvent { get; }
}

public interface IDomainEventNotification : INotification
{
    public Guid Id { get; set; }
}