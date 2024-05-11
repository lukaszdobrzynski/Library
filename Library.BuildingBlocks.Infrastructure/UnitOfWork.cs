using Microsoft.EntityFrameworkCore;

namespace Library.BuildingBlocks.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    
    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}