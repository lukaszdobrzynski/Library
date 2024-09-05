namespace Library.Modules.Reservation.Infrastructure.Jobs;

internal interface IBackgroundJob
{
    Task Run();
}