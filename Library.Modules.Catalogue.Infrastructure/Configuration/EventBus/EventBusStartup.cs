﻿using Autofac;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Reservation.IntegrationEvents;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.EventBus;

internal class EventBusStartup
{
    public static void Initialize()
    {
        var scope = CatalogueCompositionRoot.BeginLifetimeScope();
        var eventBus = scope.Resolve<IEventBus>();
        
        eventBus.Subscribe(new IntegrationEventListener<HoldCancelledIntegrationEvent>());
        eventBus.Subscribe(new IntegrationEventListener<BookPlacedOnHoldIntegrationEvent>());
    }
}