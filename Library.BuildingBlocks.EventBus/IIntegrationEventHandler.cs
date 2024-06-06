namespace Library.BuildingBlocks.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IIntegrationEvent
{
    Task Handle (TIntegrationEvent integrationEvent);
}

public interface IIntegrationEventHandler {}