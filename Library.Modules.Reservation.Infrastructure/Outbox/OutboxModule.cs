using Autofac;
using Library.BuildingBlocks.Infrastructure;

namespace Library.Modules.Reservation.Infrastructure.Outbox;

internal class OutboxModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OutboxAccessor>()
            .As<IOutboxAccessor>()
            .InstancePerLifetimeScope();
    }
}