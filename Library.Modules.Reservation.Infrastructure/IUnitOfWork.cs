namespace Library.Modules.Reservation.Infrastructure;

internal interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}