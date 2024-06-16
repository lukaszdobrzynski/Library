using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Reservation.Infrastructure.Configuration.EventBus;

public class IntegrationEventHandler<T> : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public Task Handle(T integrationEvent)
    {
        //TODO: Save message to Inbox using at most once delivery approach
        throw new NotImplementedException();
    }
}