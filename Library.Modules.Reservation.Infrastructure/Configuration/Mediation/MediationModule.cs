using Autofac;
using FluentValidation;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Application.Contracts;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Mediation;

internal class MediationModule : Autofac.Module
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
            typeof(IValidator<>)
        };

        foreach (var openHandlerType in openHandlerTypes)
        {
            builder.RegisterAssemblyTypes(typeof(CommandBase).Assembly)
                .AsClosedTypesOf(openHandlerType)
                .InstancePerLifetimeScope();
        }

        builder.RegisterAssemblyTypes(typeof(IReservationModule).Assembly)
            .AsClosedTypesOf(typeof(IDomainNotification<>))
            .InstancePerDependency();

        builder.RegisterType<DomainNotificationsRegistry>()
            .As<IDomainNotificationsRegistry>()
            .SingleInstance();
    }
}