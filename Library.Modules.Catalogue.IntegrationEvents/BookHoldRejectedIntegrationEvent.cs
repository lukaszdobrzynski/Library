using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Catalogue.IntegrationEvents;

public class BookHoldRejectedIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
}