using Autofac;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Library.Modules.Reservation.Infrastructure.Configuration.Processing;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure;

public class ReservationModule : IReservationModule
{
    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using (var scope = ReservationCompositionRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(query);
        }
    }
}