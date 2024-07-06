using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Catalogue.IntegrationEvents;

public class BookHoldRejectedIntegrationEvent : IntegrationEvent
{
    public Guid RequestHoldId { get; set; }
}