using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Application.Outbox;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Library.Modules.Reservation.Infrastructure.Outbox;
using MediatR;
using Newtonsoft.Json;

namespace Library.Modules.Reservation.Infrastructure.Events;

internal class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IDomainEventsAccessor _domainEventsAccessor;
    private readonly IMediator _mediator;
    private readonly IDomainEventToDomainNotificationResolver _domainEventToDomainNotificationResolver;
    private readonly IOutboxAccessor _outboxAccessor;
    private readonly IDomainNotificationsRegistry _notificationsRegistry;
    
    public DomainEventsDispatcher(
        IDomainEventsAccessor domainEventsAccessor, 
        IMediator mediator,
        IDomainEventToDomainNotificationResolver domainEventNotificationResolver,
        IOutboxAccessor outboxAccessor,
        IDomainNotificationsRegistry notificationsRegistry)
    {
        _domainEventsAccessor = domainEventsAccessor;
        _mediator = mediator;
        _domainEventToDomainNotificationResolver = domainEventNotificationResolver;
        _outboxAccessor = outboxAccessor;
        _notificationsRegistry = notificationsRegistry;
    }
    
    public async Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventsAccessor.GetAllDomainEvents();

        var domainNotifications = new List<IDomainNotification<IDomainEvent>>();
        
        foreach (var domainEvent in domainEvents)
        {
            var domainNotification = _domainEventToDomainNotificationResolver.ResolveOptional(domainEvent);

            if (domainNotification is not null)
            {
                domainNotifications.Add(domainNotification);                
            }
        }
        
        _domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

        foreach (var domainNotification in domainNotifications)
        {
            var type = _notificationsRegistry.GetName(domainNotification.GetType());
            var data = JsonConvert.SerializeObject(domainNotification);

            var outboxMessage = new OutboxMessage(domainNotification.Id,
                domainNotification.DomainEvent.OccurredOn,
                type,
                data);

            _outboxAccessor.Add(outboxMessage);
        }
    }
}