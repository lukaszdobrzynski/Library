using MediatR;

namespace Library.BuildingBlocks.EventBus;

public interface IIntegrationEvent : INotification
{
    Guid Id { get; set; }
    DateTime OccurredOn { get; set; }
}