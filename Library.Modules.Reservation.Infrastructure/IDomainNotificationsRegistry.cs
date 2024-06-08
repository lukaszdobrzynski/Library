namespace Library.Modules.Reservation.Infrastructure;

public interface IDomainNotificationsRegistry
{
    string GetName(Type type);

    Type GetType(string name);
}