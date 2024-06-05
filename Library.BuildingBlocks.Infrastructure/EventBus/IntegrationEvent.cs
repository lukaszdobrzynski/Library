namespace Library.BuildingBlocks.Infrastructure.EventBus;

public abstract class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
}