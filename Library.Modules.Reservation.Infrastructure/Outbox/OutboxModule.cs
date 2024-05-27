using Autofac;
using Library.BuildingBlocks.Infrastructure;

namespace Library.Modules.Reservation.Infrastructure.Outbox;

public class OutboxModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OutboxAccessor>()
            .As<IOutboxAccessor>()
            .InstancePerLifetimeScope();
    }
}