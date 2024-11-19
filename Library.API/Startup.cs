using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Library.API.ExecutionContext;
using Library.API.Modules.Catalogue;
using Library.API.Modules.Reservation;
using Library.BuildingBlocks.Application;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.Infrastructure.Configuration;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;

namespace Library.API;

public class Startup
{
    private static ILogger _logger;
    private readonly IConfiguration _configuration;
    
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        ConfigureLogger();
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        var settings = new Settings();
        _configuration.Bind(settings);
        
        ValidateSettings(settings);

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
        services.AddSingleton(settings);
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
        var settings = container.Resolve<Settings>();
        var documentStoreHolder = new DocumentStoreHolder(settings.Raven);
        
        ReservationStartup.Init(
            settings.Postgres.ConnectionString, 
            executionContextAccessor, 
            _logger, 
            eventBus);

        CatalogueStartup.Init(documentStoreHolder, executionContextAccessor, _logger, eventBus);
    }

    private void ValidateSettings(Settings settings)
    {
        if (settings.Raven is null)
        {
            throw new ApplicationConfigurationException("Raven settings cannot be null.");
        }

        if (settings.Raven.Urls is null || settings.Raven.Urls.Any() == false)
        {
            throw new ApplicationConfigurationException("Raven connection url must be provided in the application settings.");
        }

        if (string.IsNullOrWhiteSpace(settings.Raven.DatabaseName))
        {
            throw new ApplicationConfigurationException("Raven database name must be provided in the application settings.");
        }

        if (settings.Postgres is null)
        {
            throw new ApplicationConfigurationException("Postgres settings cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(settings.Postgres.ConnectionString))
        {
            throw new ApplicationConfigurationException("Postgres connection string must be provided in the application settings.");
        }
    }
}