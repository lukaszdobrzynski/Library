using Microsoft.EntityFrameworkCore;

namespace Library.BuildingBlocks.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;
    
    public UnitOfWork(DbContext dbContext, IDomainEventsDispatcher domainEventsDispatcher)
    {
        _dbContext = dbContext;
        _domainEventsDispatcher = domainEventsDispatcher;
    }
    
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        await _domainEventsDispatcher.DispatchEventsAsync();
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}