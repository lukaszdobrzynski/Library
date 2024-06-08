using Polly.Retry;

namespace Library.BuildingBlocks.Infrastructure;

public interface IRetryPolicyFactory
{
    AsyncRetryPolicy RetryWithExponentialSleepDuration(int retryCount = 5);
    AsyncRetryPolicy RetryWithSleepDuration(int retryCount, TimeSpan sleepDuration);
    public AsyncRetryPolicy RetryOnDbUpdateConcurrencyException(int retryCount);
    AsyncRetryPolicy WaitAndRetryForever(TimeSpan sleepDuration, Action<Exception> exceptionHandler);
}