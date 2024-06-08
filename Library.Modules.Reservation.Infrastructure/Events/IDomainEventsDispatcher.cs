namespace Library.Modules.Reservation.Infrastructure;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}