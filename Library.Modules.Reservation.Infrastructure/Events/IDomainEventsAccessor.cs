using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Infrastructure.Events;

internal interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}