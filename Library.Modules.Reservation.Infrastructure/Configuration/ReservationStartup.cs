using Autofac;
using Library.BuildingBlocks.Application;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.Logging;
using Library.Modules.Reservation.Infrastructure.Configuration.Mediation;
using Library.Modules.Reservation.Infrastructure.Configuration.Processing;
using ILogger = Serilog.ILogger;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

public static class ReservationStartup
{
    private static IContainer _container;
    
    public static void Init(string databaseConnectionString, IExecutionContextAccessor executionContextAccessor, ILogger logger)
    {
        var reservationModuleLogger =  logger.ForContext("Module", "Reservation");
        ConfigureContainer(databaseConnectionString, executionContextAccessor, reservationModuleLogger);
    }

    private static void ConfigureContainer(string databaseConnectionString, IExecutionContextAccessor executionContextAccessor, ILogger logger)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new LoggingModule(logger));
        containerBuilder.RegisterModule(new DataAccessModule(databaseConnectionString));
        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new MediationModule());
        
        containerBuilder.RegisterInstance(executionContextAccessor);

        _container = containerBuilder.Build();
        
        ReservationCompositionRoot.SetContainer(_container);
    }
}