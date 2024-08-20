using Library.BuildingBlocks.Domain;
using MediatR;

namespace Library.Modules.Reservation.Application.Contracts;

public interface IDomainNotification<out TEventType> : IDomainNotification
 where TEventType : IDomainEvent
{
    TEventType DomainEvent { get; }
}

public interface IDomainNotification : INotification
{
    public Guid Id { get; set; }
}