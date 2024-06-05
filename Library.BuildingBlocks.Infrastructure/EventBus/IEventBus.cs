namespace Library.BuildingBlocks.Infrastructure.EventBus;

public interface IEventBus
{
    Task Publish<T>(T integrationEvent) where T : IIntegrationEvent;

    void Subscribe<T>(IIntegrationEventHandler<T> eventHandler)
        where T : IIntegrationEvent;
}