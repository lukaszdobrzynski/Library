using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.IntegrationEvents;
using MediatR;

namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class BookHoldGrantedNotificationHandler : INotificationHandler<BookHoldGrantedNotification>
{
    private readonly IEventBus _eventBus;

    public BookHoldGrantedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public async Task Handle(BookHoldGrantedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookHoldGrantedIntegrationEvent
        {
            Id = notification.Id,
            OccurredOn = notification.OccurredOn,
            RequestHoldId = notification.ExternalHoldRequestId
        });
    }
}