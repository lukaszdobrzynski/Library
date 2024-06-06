namespace Library.BuildingBlocks.EventBus;

public interface IIntegrationEvent
{
    Guid Id { get; set; }
    DateTime OccurredOn { get; set; }
}