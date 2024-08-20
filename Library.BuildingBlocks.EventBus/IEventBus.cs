namespace Library.BuildingBlocks.EventBus;

public interface IEventBus
{
    Task Publish<T>(T integrationEvent) where T : IIntegrationEvent;

    void Subscribe<T>(IIntegrationEventListener<T> eventListener)
        where T : IIntegrationEvent;
}