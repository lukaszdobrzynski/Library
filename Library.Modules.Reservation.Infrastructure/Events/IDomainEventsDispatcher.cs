namespace Library.Modules.Reservation.Infrastructure.Events;

internal interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}