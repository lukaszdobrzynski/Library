using Autofac;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Catalogue.Infrastructure.Inbox;

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
    }
}