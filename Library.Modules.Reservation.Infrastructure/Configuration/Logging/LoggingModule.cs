using Autofac;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Logging;

internal class LoggingModule : Autofac.Module
{
    private readonly ILogger _logger;
    
    public LoggingModule(ILogger logger)
    {
        _logger = logger;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_logger)
            .As<ILogger>()
            .SingleInstance();
    }
}