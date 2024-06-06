using Autofac;
using Autofac.Core.Lifetime;
using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Reservation.Infrastructure.Configuration.EventBus;

public class EventBusStartup
{
    public static void Initialize(LifetimeScope scope)
    {
        var eventBus = scope.Resolve<IEventBus>();

        //eventBus.Subscribe();
    }
}