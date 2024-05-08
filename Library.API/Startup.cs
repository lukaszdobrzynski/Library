using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Library.API.Modules.Reservation;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Library.API;

public class Startup
{
    public Startup(IWebHostEnvironment env)
    {
        
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        InitializeModules();
    }
    
    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new ReservationRootModule());
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        var container = app.ApplicationServices.GetAutofacRoot();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private void InitializeModules()
    {
        ReservationStartup.Init();
    }
}