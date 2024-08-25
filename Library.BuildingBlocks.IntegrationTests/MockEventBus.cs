using System.Threading.Tasks;
using Library.BuildingBlocks.EventBus;

namespace Library.BuildingBlocks.IntegrationTests;

public class MockEventBus : IEventBus
{
    public Task Publish<T>(T integrationEvent) where T : IIntegrationEvent
    {
        return Task.CompletedTask;
    }

    public void Subscribe<T>(IIntegrationEventListener<T> eventListener) where T : IIntegrationEvent
    {
    }
}