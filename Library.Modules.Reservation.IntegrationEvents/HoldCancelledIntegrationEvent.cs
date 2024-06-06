using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Reservation.IntegrationEvents;

public class HoldCancelledIntegrationEvent : IntegrationEvent
{
    public Guid HoldId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public Guid PatronId { get; set; }
}