using Library.BuildingBlocks.Domain;

namespace Library.BuildingBlocks.Infrastructure;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}