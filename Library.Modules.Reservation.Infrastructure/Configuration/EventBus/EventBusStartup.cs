using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.IntegrationEvents;

namespace Library.Modules.Reservation.Infrastructure.Configuration.EventBus;

public class EventBusStartup
{
    public static void Initialize()
    {
        var scope = ReservationCompositionRoot.BeginLifetimeScope();
        var eventBus = scope.Resolve<IEventBus>();
        
        eventBus.Subscribe(new IntegrationEventListener<BookHoldRejectedIntegrationEvent>());
        eventBus.Subscribe(new IntegrationEventListener<BookHoldGrantedIntegrationEvent>());
    }
}