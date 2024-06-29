using Autofac;
using Library.Modules.Reservation.Infrastructure.Configuration;
using ILogger = Serilog.ILogger;
using Timer = System.Timers.Timer;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public static class JobsStartup
{
    public static void Initialize(ILogger logger)
    {
        RunJobInIntervals<ProcessOutboxJob>(logger, TimeSpan.FromSeconds(5));
        RunJobInIntervals<ProcessInboxJob>(logger, TimeSpan.FromSeconds(5));
        RunJobInIntervals<ProcessInternalCommandJob>(logger, TimeSpan.FromSeconds(5));
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
        var isProcessing = false;
        
        timer.Elapsed += async (sender, args) =>
        {
            if (isProcessing)
            {
                logger.Information($"Still running {taskName}. Skipping current tick...");
                return;
            }

            isProcessing = true;

            try
            {
                await backgroundJob.Run();
            }
            catch (Exception e)
            {
                logger.Error(e, $"Error running {taskName}");
            }
            finally
            {
                isProcessing = false;
            }
        };
        timer.Enabled = true;
    }
}