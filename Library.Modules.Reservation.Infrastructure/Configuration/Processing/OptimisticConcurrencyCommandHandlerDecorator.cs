using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Application.Contracts;
using MediatR;
using Polly.Retry;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class OptimisticConcurrencyCommandHandlerDecorator<T> : IRequestHandler<T>
    where T : ICommand
{
    private readonly IRequestHandler<T> _decorated;
    private readonly AsyncRetryPolicy _retryPolicy;
    
    public OptimisticConcurrencyCommandHandlerDecorator(IRequestHandler<T> decorated, IRetryPolicyFactory retryPolicyFactory)
    {
        _decorated = decorated;
        _retryPolicy = retryPolicyFactory.RetryOnceOnDbUpdateConcurrencyException();
    }
    
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(() => _decorated.Handle(command, cancellationToken));
    }
}