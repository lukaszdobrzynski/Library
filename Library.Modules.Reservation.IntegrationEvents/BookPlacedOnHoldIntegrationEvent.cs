using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Reservation.IntegrationEvents;

public class BookPlacedOnHoldIntegrationEvent : IntegrationEvent
{
    public Guid BookId { get; set; }
    public Guid PatronId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public DateTime? Till { get; set; }
}