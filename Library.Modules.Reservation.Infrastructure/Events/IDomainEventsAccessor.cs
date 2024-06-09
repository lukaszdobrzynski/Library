using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Infrastructure.Events;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}