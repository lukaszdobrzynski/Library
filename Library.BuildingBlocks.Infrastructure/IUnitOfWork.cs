namespace Library.BuildingBlocks.Infrastructure;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}