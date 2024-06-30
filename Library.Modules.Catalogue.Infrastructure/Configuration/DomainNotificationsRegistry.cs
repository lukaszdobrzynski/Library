using Library.Modules.Catalogue.Application;
using Library.Modules.Catalogue.Application.CancelHold;
using Library.Modules.Catalogue.Application.PlaceBookOnHold;
using Library.Modules.Catalogue.Application.RestoreBookAvailability;

namespace Library.Modules.Catalogue.Infrastructure.Configuration;

public class DomainNotificationsRegistry : IDomainNotificationsRegistry
{
    private readonly Dictionary<string, Type> _entries = new ()
    {
        { $"{nameof(BookHoldGrantedNotification)}", typeof(BookHoldGrantedNotification) },
        { $"{nameof(BookHoldRejectedNotification)}", typeof(BookHoldRejectedNotification) },
        { $"{nameof(BookMadeAvailableForReservationNotification)}", typeof(BookMadeAvailableForReservationNotification) }
    };

    public string GetName(Type type) => _entries.Single(x => x.Value == type).Key;

    public Type GetType(string name) => _entries[name];
}