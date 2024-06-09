using Library.BuildingBlocks.Domain;
using MediatR;

namespace Library.Modules.Reservation.Application.Contracts;

public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
 where TEventType : IDomainEvent
{
    TEventType DomainEvent { get; }
}

public interface IDomainEventNotification : INotification
{
    public Guid Id { get; set; }
}