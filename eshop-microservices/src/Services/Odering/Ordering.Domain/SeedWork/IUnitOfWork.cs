﻿namespace Ordering.Domain.SeedWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
