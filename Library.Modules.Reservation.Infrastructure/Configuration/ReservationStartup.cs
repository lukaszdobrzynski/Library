using Autofac;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.Mediation;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

public static class ReservationStartup
{
    private static IContainer _container;
    
    public static void Init()
    {
        ConfigureContainer();
    }

    private static void ConfigureContainer()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new MediationModule());
        containerBuilder.RegisterModule(new DataAccessModule());

        _container = containerBuilder.Build();
        
        ReservationCompositionRoot.SetContainer(_container);
    }
}