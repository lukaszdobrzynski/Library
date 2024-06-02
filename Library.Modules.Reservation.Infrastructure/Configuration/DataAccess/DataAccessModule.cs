using Autofac;
using Library.BuildingBlocks.Application.Data;
using Library.BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;

public class DataAccessModule : Autofac.Module
{
    private readonly string _databaseConnectionString;
    
    public DataAccessModule(string databaseConnectionString)
    {
        _databaseConnectionString = databaseConnectionString;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PsqlConnectionFactory>()
            .As<IPsqlConnectionFactory>()
            .WithParameter("connectionString", _databaseConnectionString)
            .InstancePerLifetimeScope();
        
        builder
            .Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<ReservationContext>();
                dbContextOptionsBuilder.UseNpgsql(_databaseConnectionString);

                dbContextOptionsBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new ReservationContext(dbContextOptionsBuilder.Options);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();
        
        builder.RegisterAssemblyTypes(typeof(ReservationModule).Assembly)
            .Where(x => x.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}