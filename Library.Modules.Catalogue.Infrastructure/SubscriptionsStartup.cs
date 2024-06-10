using Autofac;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Catalogue.Infrastructure.Configuration;
using Library.Modules.Catalogue.Infrastructure.Inbox;
using Polly.Retry;
using Serilog;

namespace Library.Modules.Catalogue.Infrastructure;

public class SubscriptionsStartup
{
    private const int TaskStartDelayInSeconds = 10;
    private static readonly TimeSpan RetrySleepDuration = TimeSpan.FromMinutes(1);
    
    public static void Initialize(ILogger logger)
    {
        RunSubscription<InboxMessagesSubscriptionProcessor>(logger);
    }
    
    private static void RunSubscription<T>(ILogger logger) where T : ISubscriptionProcessor
    {
        var scope = CatalogueCompositionRoot.BeginLifetimeScope();
        var processor = scope.Resolve<T>();
        var policyFactory = scope.Resolve<IRetryPolicyFactory>();
        var retryPolicy = policyFactory.WaitAndRetryForever(RetrySleepDuration, ex =>
        {
            LogCriticalSubscriptionError<T>(ex, logger);
        });
            
        RunSubscription<T>(retryPolicy, processor, logger);
    }
    
    private static void RunSubscription<T>(AsyncRetryPolicy retryPolicy, ISubscriptionProcessor processor, ILogger logger)
    {
        Task.Run(async () =>
        {
            try
            {
                await retryPolicy.ExecuteAsync(() => TryRunSubscription(processor));
            }
            catch (Exception e)
            {
                logger.Fatal(e, "A critical error occured while retrying to run subscription.");
            }
        });
    }
    
    private static async Task TryRunSubscription(ISubscriptionProcessor processor)
    {
        await Task.Delay(TimeSpan.FromSeconds(TaskStartDelayInSeconds));
        await processor.Run();   
    }

    private static void LogCriticalSubscriptionError<T>(Exception exception, ILogger logger)
    {
        var subscriptionName = typeof(T).ToString();
        var error = $"Error running {subscriptionName}. Retrying until successful...";
        logger.Fatal(exception, error);
    }
}