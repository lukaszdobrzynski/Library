using Autofac;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Inbox;
using Library.Modules.Catalogue.Infrastructure.Outbox;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Processing;

public class ProcessingModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CatalogueRetryPolicyFactory>()
            .As<IRetryPolicyFactory>()
            .InstancePerLifetimeScope();

        builder.RegisterType<InboxMessagesSubscriptionProcessor>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<InboxMessageHandlingStrategy>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<InternalCommandsSubscriptionProcessor>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<InternalCommandHandlingStrategy>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<OutboxMessageSubscriptionProcessor>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<OutboxMessageHandlingStrategy>()
            .AsSelf()
            .SingleInstance();
        
        var openHandlerTypes = new[]
        {
            typeof(InternalCommandHandler<>)
        };

        foreach (var openHandlerType in openHandlerTypes)
        {
            builder.RegisterAssemblyTypes(typeof(InternalCommandBase).Assembly)
                .AsClosedTypesOf(openHandlerType)
                .InstancePerLifetimeScope();
        }

        builder.RegisterGenericDecorator(typeof(OptimisticConcurrencyCommandHandlerDecorator<>),
            typeof(InternalCommandHandler<>));
    }
}