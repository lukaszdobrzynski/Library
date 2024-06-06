using Autofac;
using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Reservation.Infrastructure.Configuration.EventBus;

public class EventBusModule : Autofac.Module
{
    private readonly IEventBus _eventBus;
    
    public EventBusModule(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_eventBus)
            .SingleInstance();
    }
}