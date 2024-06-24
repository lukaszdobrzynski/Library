namespace Library.Modules.Reservation.Application.Contracts;

public interface IInternalCommandsScheduler
{
    Task Submit(ICommand command);
}