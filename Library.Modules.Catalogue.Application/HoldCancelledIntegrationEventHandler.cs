using Library.Modules.Reservation.IntegrationEvents;
using MediatR;

namespace Library.Modules.Catalogue.Application;

public class HoldCancelledIntegrationEventHandler : INotificationHandler<HoldCancelledIntegrationEvent>
{
    public Task Handle(HoldCancelledIntegrationEvent notificationEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}