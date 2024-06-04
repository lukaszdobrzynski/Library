using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Application.Patrons.CancelHold;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

public class DomainNotificationsRegistry : IDomainNotificationsRegistry
{
    private readonly Dictionary<string, Type> _entries = new ()
    {
        { $"{nameof(HoldCanceledNotification)}", typeof(HoldCanceledNotification) }
    };

    public string GetName(Type type) => _entries.Single(x => x.Value == type).Key;

    public Type GetType(string name) => _entries[name];
}