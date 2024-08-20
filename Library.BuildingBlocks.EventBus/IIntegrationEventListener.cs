namespace Library.BuildingBlocks.EventBus;

public interface IIntegrationEventListener<in TIntegrationEvent> : IIntegrationEventListener
    where TIntegrationEvent : IIntegrationEvent
{
    Task Register (TIntegrationEvent integrationEvent);
}

public interface IIntegrationEventListener {}