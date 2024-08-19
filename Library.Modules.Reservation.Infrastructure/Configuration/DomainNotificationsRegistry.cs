using Library.Modules.Reservation.Application.Holds.CancelHold;
using Library.Modules.Reservation.Application.Patrons.CancelHold;
using Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

namespace Library.Modules.Reservation.Infrastructure.Configuration
{
    public class DomainNotificationsRegistry : IDomainNotificationsRegistry
    {
        private readonly Dictionary<string, Type> _entries = new ()
        {
            { $"{nameof(HoldCanceledNotification)}", typeof(HoldCanceledNotification) },
            { $"{nameof(BookPlacedOnHoldNotification)}", typeof(BookPlacedOnHoldNotification) },
            { $"{nameof(CancelHoldDecisionAppliedNotification)}", typeof(CancelHoldDecisionAppliedNotification) }
        };

        public string GetName(Type type) => _entries.Single(x => x.Value == type).Key;

        public Type GetType(string name) => _entries[name];
    }
}