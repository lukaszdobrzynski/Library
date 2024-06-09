namespace Library.Modules.Reservation.Infrastructure.Events;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}