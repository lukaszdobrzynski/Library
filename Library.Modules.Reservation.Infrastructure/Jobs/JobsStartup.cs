using Autofac;
using Library.Modules.Reservation.Infrastructure.Configuration;
using ILogger = Serilog.ILogger;
using Timer = System.Timers.Timer;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public static class JobsStartup
{
    public static void Initialize(ILogger logger)
    {
        RunJobInIntervals<ProcessOutboxJob>(logger, TimeSpan.FromSeconds(10));
        RunJobInIntervals<ProcessInboxJob>(logger, TimeSpan.FromSeconds(10));
    }
    
    private static void RunJobInIntervals<TBackgroundJob>(
        ILogger logger,
        TimeSpan taskInterval)
        where TBackgroundJob : IBackgroundJob
    {
        var backgroundJob = ReservationCompositionRoot.BeginLifetimeScope()
            .Resolve<TBackgroundJob>();
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