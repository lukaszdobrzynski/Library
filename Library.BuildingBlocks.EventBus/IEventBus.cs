namespace Library.BuildingBlocks.EventBus;

public interface IEventBus
{
    Task Publish<T>(T integrationEvent) where T : IIntegrationEvent;

    void Subscribe<T>(IIntegrationEventHandler<T> eventHandler)
        where T : IIntegrationEvent;
}