using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Catalogue.IntegrationEvents;

public class BookHoldGrantedIntegrationEvent : IntegrationEvent
{
    public Guid RequestHoldId { get; set; }
}