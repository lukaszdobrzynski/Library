using Library.BuildingBlocks.Infrastructure;
using Polly;
using Polly.Retry;
using Raven.Client.Exceptions;

namespace Library.Modules.Catalogue.Infrastructure;

public class CatalogueRetryPolicyFactory : RetryPolicyFactory
{
    public override AsyncRetryPolicy RetryOnDbUpdateConcurrencyException(int retryCount)
    {
        var policy = Policy
            .Handle<Exception>(e => e is ConcurrencyException)
            .WaitAndRetryAsync(1, (_) => TimeSpan.FromMilliseconds(1500));
            
        return policy;
    }
}