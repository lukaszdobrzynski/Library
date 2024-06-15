namespace Library.Modules.Reservation.Infrastructure.Configuration
{
    public interface IDomainNotificationsRegistry
    {
        string GetName(Type type);

        Type GetType(string name);
    }
}