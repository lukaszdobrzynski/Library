using Autofac;
using Library.BuildingBlocks.Infrastructure;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class ProcessingModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainEventsAccessor>()
            .As<IDomainEventsAccessor>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterGenericDecorator(typeof(UnitOfWorkCommandHandlerDecorator<>), 
            typeof(IRequestHandler<>));
    }
}