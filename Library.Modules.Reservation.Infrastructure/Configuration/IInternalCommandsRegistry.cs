namespace Library.Modules.Reservation.Infrastructure.Configuration;

public interface IInternalCommandsRegistry
{
    string GetName(Type type);

    Type GetType(string name);
}