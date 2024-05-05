using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Infrastructure;

public class ReservationModule : IReservationModule
{
    public Task ExecuteCommandAsync(ICommand command)
    {
        throw new NotImplementedException();
    }

    public Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        throw new NotImplementedException();
    }

    public Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        throw new NotImplementedException();
    }
}