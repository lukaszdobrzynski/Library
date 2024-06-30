using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.IntegrationEvents;
using MediatR;

namespace Library.Modules.Catalogue.Application.RestoreBookAvailability;

public class BookMadeAvailableForReservationNotificationHandler : INotificationHandler<BookMadeAvailableForReservationNotification>
{
    private readonly IEventBus _eventBus;
    
    public BookMadeAvailableForReservationNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public async Task Handle(BookMadeAvailableForReservationNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookMadeAvailableForReservationIntegrationEvent
        {
            Id = Guid.NewGuid(),
            BookId = notification.BookId,
            LibraryBranchId = notification.LibraryBranchId,
            OccurredOn = DateTime.UtcNow
        });
    }
}