using Library.BuildingBlocks.Infrastructure;
using Polly.Retry;

namespace Library.Modules.Catalogue.Infrastructure;

public class CatalogueRetryPolicyFactory : RetryPolicyFactory
{
    public override AsyncRetryPolicy RetryOnDbUpdateConcurrencyException(int retryCount)
    {
        throw new NotImplementedException();
    }
}