using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Library.API.ExecutionContext;
using Library.API.Modules.CatalogueRootModule;
using Library.API.Modules.Reservation;
using Library.BuildingBlocks.Application;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;

namespace Library.API;

public class Startup
{
    private static ILogger _logger;
    
    public Startup(IWebHostEnvironment env)
    {
        ConfigureLogger();
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
    }
    
    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new ReservationRootModule());
        containerBuilder.RegisterModule(new CatalogueRootModule());
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        var container = app.ApplicationServices.GetAutofacRoot();
        
        InitializeModules(container);

        app.UseMiddleware<CorrelationMiddleware>();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private void ConfigureLogger()
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{Module}] [{Context}] {Message:lj} {NewLine} {Exception}")
            .WriteTo.RollingFile(new CompactJsonFormatter(), Path.Combine(AppContext.BaseDirectory, "logs/logs"))
            .CreateLogger();
    }

    private void InitializeModules(ILifetimeScope container)
    {
        var eventBus = new InMemoryEventBus();
        var executionContextAccessor = container.Resolve<IExecutionContextAccessor>();
        var ravenSettings = new RavenDatabaseSettings
        {
            Urls = new[] { "http://localhost:8080", "http://localhost:8081", "http://localhost:8082" },
            DatabaseName = "Library.Catalogue"
        };
        
        ReservationStartup.Init(
            "Host=localhost;Port=5432;Database=library;Username=postgres;Password=admin", 
            executionContextAccessor, 
            _logger, 
            eventBus);

        CatalogueStartup.Init(ravenSettings, executionContextAccessor, _logger, eventBus);
    }
}