using Library.Modules.Catalogue.IntegrationEvents;
using MediatR;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class BookHoldGrantedIntegrationEventHandler : INotificationHandler<BookHoldGrantedIntegrationEvent>
{
    public Task Handle(BookHoldGrantedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}