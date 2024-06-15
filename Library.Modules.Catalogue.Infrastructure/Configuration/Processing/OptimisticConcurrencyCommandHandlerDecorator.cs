using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Catalogue.Application.Contracts;
using Polly.Retry;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Processing;

public class OptimisticConcurrencyCommandHandlerDecorator<TCommand> : InternalCommandHandler<TCommand>
    where TCommand : InternalCommandBase
{
    private readonly InternalCommandHandler<TCommand> _decorated;
    private readonly AsyncRetryPolicy _retryPolicy;

    private const int RetryCount = 1;
    
    public OptimisticConcurrencyCommandHandlerDecorator(InternalCommandHandler<TCommand> decorated, 
        IRetryPolicyFactory retryPolicyFactory)
    {
        _decorated = decorated;
        _retryPolicy = retryPolicyFactory.RetryOnDbUpdateConcurrencyException(RetryCount);
    }
    
    protected override async Task HandleConcreteCommand(TCommand command)
    {
        await _retryPolicy.ExecuteAsync(() => _decorated.Handle(command));
    }
}
