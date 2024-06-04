using MediatR;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class HoldCancelledNotificationHandler : INotificationHandler<HoldCanceledNotification>
{
    public Task Handle(HoldCanceledNotification notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}