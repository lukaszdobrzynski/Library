using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Infrastructure;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}