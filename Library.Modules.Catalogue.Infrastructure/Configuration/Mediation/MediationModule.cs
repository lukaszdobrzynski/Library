using Autofac;
using FluentValidation;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Catalogue.Application.Contracts;
using MediatR;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Mediation;

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
            builder.RegisterAssemblyTypes(typeof(InternalCommandBase).Assembly)
                .AsClosedTypesOf(openHandlerType)
                .InstancePerLifetimeScope();
        }
    }
}