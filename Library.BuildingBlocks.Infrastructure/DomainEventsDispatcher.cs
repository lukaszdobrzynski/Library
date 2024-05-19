using MediatR;

namespace Library.BuildingBlocks.Infrastructure;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IDomainEventsAccessor _domainEventsAccessor;
    private readonly IMediator _mediator;
    
    public DomainEventsDispatcher(IDomainEventsAccessor domainEventsAccessor, IMediator mediator)
    {
        _domainEventsAccessor = domainEventsAccessor;
        _mediator = mediator;
    }
    
    public Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventsAccessor.GetAllDomainEvents();
        
        _domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            _mediator.Publish(domainEvent);
        }
        
        //TODO dispatch notifications
        
        return Task.CompletedTask;
    }
}