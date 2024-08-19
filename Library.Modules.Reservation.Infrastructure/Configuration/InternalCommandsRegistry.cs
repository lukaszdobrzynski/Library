using Library.Modules.Reservation.Application.Holds.CancelHold;
using Library.Modules.Reservation.Application.Holds.GrantHold;
using Library.Modules.Reservation.Application.Holds.RejectHold;

namespace Library.Modules.Reservation.Infrastructure.Configuration;

public class InternalCommandsRegistry : IInternalCommandsRegistry
{
    private readonly Dictionary<string, Type> _entries = new ()
    {
        { $"{nameof(ApplyGrantHoldDecisionCommand)}", typeof(ApplyGrantHoldDecisionCommand) },
        { $"{nameof(ApplyRejectHoldDecisionCommand)}", typeof(ApplyRejectHoldDecisionCommand) },
        { $"{nameof(CancelExpiredHoldCommand)}", typeof(CancelExpiredHoldCommand)} 
    };

    public string GetName(Type type) => _entries.Single(x => x.Value == type).Key;

    public Type GetType(string name) => _entries[name];
}