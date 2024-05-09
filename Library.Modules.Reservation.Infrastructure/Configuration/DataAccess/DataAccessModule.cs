using Autofac;

namespace Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;

public class DataAccessModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(ReservationModule).Assembly)
            .Where(x => x.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}