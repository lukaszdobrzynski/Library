using Autofac;
using Library.BuildingBlocks.Application;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.EventBus;
using Library.Modules.Reservation.Infrastructure.Configuration.Logging;
using Library.Modules.Reservation.Infrastructure.Configuration.Mediation;
using Library.Modules.Reservation.Infrastructure.Configuration.Processing;
using Library.Modules.Reservation.Infrastructure.Jobs;
using Library.Modules.Reservation.Infrastructure.Outbox;
using ILogger = Serilog.ILogger;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

public static class ReservationStartup
{
    private static IContainer _container;
    
    public static void Init(string databaseConnectionString, IExecutionContextAccessor executionContextAccessor, ILogger logger, IEventBus eventBus)
    {
        var reservationModuleLogger =  logger.ForContext("Module", "Reservation");
        
        ConfigureContainer(databaseConnectionString, executionContextAccessor, reservationModuleLogger, eventBus);
        
        JobsStartup.Initialize(reservationModuleLogger);
        EventBusStartup.Initialize();
    }

    private static void ConfigureContainer(string databaseConnectionString, IExecutionContextAccessor executionContextAccessor, ILogger logger, IEventBus eventBus)
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(new LoggingModule(logger));
        containerBuilder.RegisterModule(new DataAccessModule(databaseConnectionString));
        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new MediationModule());
        containerBuilder.RegisterModule(new EventBusModule(eventBus));
        containerBuilder.RegisterModule(new OutboxModule());
        
        containerBuilder.RegisterInstance(executionContextAccessor);

        _container = containerBuilder.Build();
        
        ReservationCompositionRoot.SetContainer(_container);
    }
}