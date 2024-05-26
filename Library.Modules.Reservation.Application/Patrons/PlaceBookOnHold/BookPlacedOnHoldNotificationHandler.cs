using MediatR;

namespace Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

public class BookPlacedOnHoldNotificationHandler : INotificationHandler<BookPlacedOnHoldNotification>
{
    public Task Handle(BookPlacedOnHoldNotification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}