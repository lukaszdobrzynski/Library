﻿using Autofac;
using Library.BuildingBlocks.EventBus;

namespace Library.Modules.Reservation.Infrastructure.Configuration.EventBus;

public class EventBusStartup
{
    public static void Initialize()
    {
        var scope = ReservationCompositionRoot.BeginLifetimeScope();
        var eventBus = scope.Resolve<IEventBus>();
    }
}