namespace Library.BuildingBlocks.EventBus;

public class InMemoryEventBus : IEventBus
{
    private readonly Dictionary<string, List<IIntegrationEventHandler>> _registry = new();

    public async Task Publish<T>(T integrationEvent) where T : IIntegrationEvent
    {
        var eventName = integrationEvent.GetType().FullName;

        if (eventName is not null)
        {
            var handlers = _registry[eventName];

            foreach (var handler in handlers)
            {
                if (handler is IIntegrationEventHandler<T> eventHandler)
                {
                    await eventHandler.Handle(integrationEvent);
                }
            }
        }
    }

    public void Subscribe<T>(IIntegrationEventHandler<T> eventHandler) where T : IIntegrationEvent
    {
        var eventName = typeof(T).FullName;

        if (eventName is null) 
            return;
        
        if (_registry.TryGetValue(eventName, out var handlers))
        {
            handlers.Add(eventHandler);
        }
        else
        {
            _registry.Add(eventName, new List<IIntegrationEventHandler> {eventHandler});
        }
    }
}