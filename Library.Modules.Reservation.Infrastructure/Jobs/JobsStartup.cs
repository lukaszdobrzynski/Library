using Autofac;
using ILogger = Serilog.ILogger;
using Timer = System.Timers.Timer;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public static class JobsStartup
{
    public static void Initialize(ILifetimeScope lifetimeScope, ILogger logger)
    {
        RunJobInIntervals<ProcessOutboxJob>(lifetimeScope, logger, TimeSpan.FromSeconds(30));
    }
    
    private static void RunJobInIntervals<TBackgroundJob>(
        ILifetimeScope lifetimeScope,
        ILogger logger,
        TimeSpan taskInterval)
        where TBackgroundJob : IBackgroundJob
    {
        var backgroundJob = lifetimeScope.Resolve<TBackgroundJob>();
        var taskName = backgroundJob.GetType().Name;

        var timer = new Timer(taskInterval);
        timer.Elapsed += async (sender, args) =>
        {
            {
                try
                {
                    await backgroundJob.Run();
                }
                catch (Exception e)
                {
                    logger.Error(e, $"Error running {taskName}");
                }
            }
        };
        timer.Enabled = true;
    }
}