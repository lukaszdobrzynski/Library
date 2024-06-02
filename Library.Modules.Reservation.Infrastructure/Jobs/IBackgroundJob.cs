namespace Library.Modules.Reservation.Infrastructure.Jobs;

public interface IBackgroundJob
{
    Task Run();
}