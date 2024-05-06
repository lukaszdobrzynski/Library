using Autofac;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure;

namespace Library.API.Modules.Reservation;

public class ReservationRootModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ReservationModule>()
            .As<IReservationModule>()
            .InstancePerLifetimeScope();
    }
}