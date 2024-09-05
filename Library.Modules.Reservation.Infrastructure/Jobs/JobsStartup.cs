using Autofac;
using Library.Modules.Reservation.Infrastructure.Configuration;
using static System.Threading.Tasks.Task;
using ILogger = Serilog.ILogger;
using Timer = System.Timers.Timer;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

internal static class JobsStartup
{
    public static void Initialize(ILogger logger)
    {
        RunJobAtIntervals<ProcessOutboxJob>(logger, TimeSpan.FromSeconds(1));
        RunJobAtIntervals<ProcessInboxJob>(logger, TimeSpan.FromSeconds(1));
        RunJobAtIntervals<ProcessInternalCommandJob>(logger, TimeSpan.FromSeconds(1));
        RunJobAtSpecificHour<ProcessDailySheetJob>(logger, Hour.Midnight);
    }

    private static void RunJobAtSpecificHour<TBackgroundJob>(ILogger logger, Hour hour)
        where TBackgroundJob : IBackgroundJob
    {
        var backgroundJob = ReservationCompositionRoot.BeginLifetimeScope()
            .Resolve<TBackgroundJob>();
        var taskName = backgroundJob.GetType().Name;
        
        Run(async () =>
        {
            await Delay(TimeSpan.FromSeconds(5));
            
            try
            {
                while(true)
                {
                    var now = DateTime.UtcNow;
            
                    if (now.Hour == (int)hour)
                    {
                        await backgroundJob.Run();
                    }
            
                    var numberOfHoursToWait = CalculateNumberOfHoursToWaitForNextJobRun(hour, now);
                    await Delay(TimeSpan.FromHours(numberOfHoursToWait));    
                }
            }
            catch (Exception e)
            {
                logger.Error(e, $"Error running {taskName}");            
            }
        });
    }
    
    private static int CalculateNumberOfHoursToWaitForNextJobRun(Hour jobStartHour, DateTime now)
    {
        var startHour = (int)jobStartHour;
        var hourNow = now.Hour;    
        
        return hourNow >= startHour
            ? startHour + (24 - hourNow)
            : startHour - hourNow;
    }
    
    private static void RunJobAtIntervals<TBackgroundJob>(
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

    private enum Hour
    {
       Midnight = 0,
       OneAm = 1,
       TwoAm = 2,
       ThreeAm = 3,
       FourAm = 4,
       FiveAm = 5,
       SixAm = 6,
       SevenAm = 7,
       EightAm = 8,
       NineAm = 9,
       TenAm = 10,
       ElevenAm = 11,
       Noon = 12,
       OnePm = 13,
       TwoPm = 14,
       ThreePm = 15,
       FourPm = 16,
       FivePm = 17,
       SixPm = 18,
       SevenPm = 19,
       EightPm = 20,
       NinePm = 21,
       TenPm = 22,
       ElevenPm = 23
    }
}