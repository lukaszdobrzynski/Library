namespace Library.BuildingBlocks.EventBus;

public class InMemoryEventBus : IEventBus
{
    private readonly Dictionary<string, List<IIntegrationEventListener>> _registry = new();

    public async Task Publish<T>(T integrationEvent) where T : IIntegrationEvent
    {
        var eventName = integrationEvent.GetType().FullName;

        if (eventName is not null)
        {
            var subscribersExist = _registry.TryGetValue(eventName, out var listeners);

            if (subscribersExist == false)
                return;

            foreach (var listener in listeners)
            {
                if (listener is IIntegrationEventListener<T> eventListener)
                {
                    await eventListener.Register(integrationEvent);
                }
            }
        }
    }

    public void Subscribe<T>(IIntegrationEventListener<T> eventListener) where T : IIntegrationEvent
    {
        var eventName = typeof(T).FullName;

        if (eventName is null) 
            return;
        
        if (_registry.TryGetValue(eventName, out var listeners))
        {
            listeners.Add(eventListener);
        }
        else
        {
            _registry.Add(eventName, new List<IIntegrationEventListener> {eventListener});
        }
    }
}