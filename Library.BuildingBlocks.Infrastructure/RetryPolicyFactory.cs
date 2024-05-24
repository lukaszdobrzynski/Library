using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Library.BuildingBlocks.Infrastructure;

public class RetryPolicyFactory : IRetryPolicyFactory
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
    
    public AsyncRetryPolicy RetryOnceOnDbUpdateConcurrencyException()
    {
        var policy = Policy
            .Handle<Exception>(e => e is DbUpdateConcurrencyException)
            .WaitAndRetryAsync(1, (_) => TimeSpan.FromMilliseconds(1500));
            
        return policy;
    }

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