using Autofac;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.Mediation;
using Library.Modules.Reservation.Infrastructure.Configuration.Processing;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

public static class ReservationStartup
{
    private static IContainer _container;
    
    public static void Init(string databaseConnectionString)
    {
        ConfigureContainer(databaseConnectionString);
    }

    private static void ConfigureContainer(string databaseConnectionString)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new DataAccessModule(databaseConnectionString));
        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new MediationModule());

        _container = containerBuilder.Build();
        
        ReservationCompositionRoot.SetContainer(_container);
    }
}