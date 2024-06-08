﻿namespace Library.Modules.Reservation.Infrastructure;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}