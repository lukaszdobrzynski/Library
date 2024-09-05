using Library.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure.Events;

internal class DomainEventsAccessor : IDomainEventsAccessor
{
    private readonly DbContext _dbContext;
    
    public DomainEventsAccessor(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        var domainEvents = _dbContext.ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        return domainEvents.SelectMany(x => x.Entity.DomainEvents)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEvents = _dbContext.ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            domainEvent.Entity.ClearDomainEvents();
        }
    }
}