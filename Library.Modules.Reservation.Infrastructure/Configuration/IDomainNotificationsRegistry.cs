namespace Library.Modules.Reservation.Infrastructure.Configuration
{
    internal interface IDomainNotificationsRegistry
    {
        string GetName(Type type);

        Type GetType(string name);
    }
}