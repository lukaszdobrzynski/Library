namespace Library.Modules.Reservation.Infrastructure.Configuration;

internal interface IInternalCommandsRegistry
{
    string GetName(Type type);

    Type GetType(string name);
}