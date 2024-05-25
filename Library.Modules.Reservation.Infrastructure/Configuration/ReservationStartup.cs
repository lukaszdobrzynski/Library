using Autofac;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.Logging;
using Library.Modules.Reservation.Infrastructure.Configuration.Mediation;
using Library.Modules.Reservation.Infrastructure.Configuration.Processing;
using ILogger = Serilog.ILogger;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

public static class ReservationStartup
{
    private static IContainer _container;
    
    public static void Init(string databaseConnectionString, ILogger logger)
    {
        ConfigureContainer(databaseConnectionString, logger);
    }

    private static void ConfigureContainer(string databaseConnectionString, ILogger logger)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new LoggingModule(logger));
        containerBuilder.RegisterModule(new DataAccessModule(databaseConnectionString));
        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new MediationModule());

        _container = containerBuilder.Build();
        
        ReservationCompositionRoot.SetContainer(_container);
    }
}