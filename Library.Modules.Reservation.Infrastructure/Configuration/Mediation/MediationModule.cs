using Autofac;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Checkouts;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Infrastructure.Domain.Books;
using Library.Modules.Reservation.Infrastructure.Domain.Checkouts;
using Library.Modules.Reservation.Infrastructure.Domain.Holds;
using Library.Modules.Reservation.Infrastructure.Domain.Patrons;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Mediation;

public class MediationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ServiceProviderWrapper>()
            .As<IServiceProvider>()
            .InstancePerDependency()
            .IfNotRegistered(typeof(IServiceProvider));
        
        builder
            .RegisterType(typeof(Mediator))
            .As<IMediator>()
            .InstancePerLifetimeScope();
        
        var openHandlerTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(IRequestHandler<>),
            typeof(INotificationHandler<>),
        };

        foreach (var openHandlerType in openHandlerTypes)
        {
            builder.RegisterAssemblyTypes(typeof(CommandBase).Assembly)
                .AsClosedTypesOf(openHandlerType)
                .InstancePerLifetimeScope();
        }

        builder.RegisterType<PatronRepository>()
            .As<IPatronRepository>()
            .SingleInstance();
        
        builder.RegisterType<HoldRepository>()
            .As<IHoldRepository>()
            .SingleInstance();

        builder.RegisterType<BookRepository>()
            .As<IBookRepository>()
            .SingleInstance();
        
        builder.RegisterType<CheckoutRepository>()
            .As<ICheckoutRepository>()
            .SingleInstance();
    }
}