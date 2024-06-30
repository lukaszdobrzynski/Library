using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Catalogue.IntegrationEvents;

public class BookMadeAvailableForReservationIntegrationEvent : IntegrationEvent
{
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
}