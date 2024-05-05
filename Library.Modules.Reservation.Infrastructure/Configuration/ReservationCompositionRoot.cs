using Autofac;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

internal static class ReservationCompositionRoot
{
    private static IContainer _container;

    public static void SetContainer(IContainer container)
    {
        _container = container;
    }

    public static ILifetimeScope BeginLifetimeScope()
    {
        return _container.BeginLifetimeScope();
    }
}