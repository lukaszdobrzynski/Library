using Autofac;
using Autofac.Core;
using Library.BuildingBlocks.Application.Events;
using Library.BuildingBlocks.Application.Outbox;
using Library.BuildingBlocks.Domain;
using MediatR;
using Newtonsoft.Json;

namespace Library.BuildingBlocks.Infrastructure;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IDomainEventsAccessor _domainEventsAccessor;
    private readonly IMediator _mediator;
    private readonly ILifetimeScope _scope;
    
    public DomainEventsDispatcher(IDomainEventsAccessor domainEventsAccessor, IMediator mediator, ILifetimeScope scope)
    {
        _domainEventsAccessor = domainEventsAccessor;
        _mediator = mediator;
        _scope = scope;

    }
    
    public Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventsAccessor.GetAllDomainEvents();

        var domainNotifications = new List<IDomainEventNotification<IDomainEvent>>();
        
        foreach (var domainEvent in domainEvents)
        {
            var domainEventNotificationType = typeof(IDomainEventNotification<>);
            var domainEventNotificationGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());
            var domainNotification = _scope.ResolveOptional(domainEventNotificationGenericType, new List<Parameter>
            {
                new NamedParameter("domainEvent", domainEvent),
                new NamedParameter("id", domainEvent.Id)
            });

            if (domainNotification is not null)
            {
                domainNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);                
            }
        }
        
        _domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            _mediator.Publish(domainEvent);
        }

        foreach (var domainNotification in domainNotifications)
        {
            var type = domainNotification.GetType().Name;  //TODO USE MAP
            var data = JsonConvert.SerializeObject(domainNotification);

            var outboxMessage = new OutboxMessage(domainNotification.Id,
                domainNotification.DomainEvent.OccurredOn,
                type,
                data);

            //TODO dispatch notifications
        }
        return Task.CompletedTask;
    }
}