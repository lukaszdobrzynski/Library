using Polly;
using Polly.Retry;

namespace Library.BuildingBlocks.Infrastructure;

public abstract class RetryPolicyFactory : IRetryPolicyFactory
{
    public AsyncRetryPolicy RetryWithExponentialSleepDuration(int retryCount = 5)
    {
        var policy = Policy
            .Handle<Exception>()
            .GetSleepRetryPolicyAsync(retryCount, GetExponentialSleepDuration);
            
        return policy;
    }

    public AsyncRetryPolicy RetryWithSleepDuration(int retryCount, TimeSpan sleepDuration)
    {
        var policy = Policy
            .Handle<Exception>()
            .GetSleepRetryPolicyAsync(retryCount, _ => sleepDuration);

        return policy;
    }

    public abstract AsyncRetryPolicy RetryOnDbUpdateConcurrencyException(int retryCount);

    public AsyncRetryPolicy WaitAndRetryForever(TimeSpan sleepDuration, Action<Exception> exceptionHandler)
    {
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(_ => sleepDuration, (ex, _) =>
            {
                exceptionHandler(ex);
            });

        return policy;
    }
    
    private TimeSpan GetExponentialSleepDuration(int count) => TimeSpan.FromSeconds(Math.Pow(2, count));
}

public static class PolicyBuilderExtensions
{
    public static AsyncRetryPolicy GetSleepRetryPolicyAsync(this PolicyBuilder builder, int retryCount, Func<int, TimeSpan> getSleepDuration) 
        => builder
            .WaitAndRetryAsync(
                retryCount,
                getSleepDuration,
                (_, _) =>
                {
                }
            );
}