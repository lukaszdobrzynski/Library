using Autofac;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure.Events;
using Library.Modules.Reservation.Infrastructure.Jobs;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class ProcessingModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ProcessOutboxJob>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<ProcessInboxJob>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<ProcessInternalCommandJob>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<ProcessDailySheetJob>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<InternalCommandsScheduler>()
            .As<IInternalCommandsScheduler>()
            .SingleInstance();

        builder.RegisterType<InternalCommandsRegistry>()
            .As<IInternalCommandsRegistry>()
            .SingleInstance();

        builder.RegisterType<DomainEventToDomainNotificationResolver>()
            .As<IDomainEventToDomainNotificationResolver>()
            .SingleInstance();
        
        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainEventsAccessor>()
            .As<IDomainEventsAccessor>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ReservationRetryPolicyFactory>()
            .As<IRetryPolicyFactory>()
            .InstancePerLifetimeScope();
        
        builder.RegisterGenericDecorator(typeof(UnitOfWorkCommandHandlerDecorator<>), 
            typeof(IRequestHandler<>));
        
        builder.RegisterGenericDecorator(typeof(OptimisticConcurrencyCommandHandlerDecorator<>),
            typeof(IRequestHandler<>));
        
        builder.RegisterGenericDecorator(typeof(ValidationCommandHandlerDecorator<>),
        typeof(IRequestHandler<>));

        builder.RegisterGenericDecorator(typeof(LoggingCommandHandlerDecorator<>),
            typeof(IRequestHandler<>));
    }
}