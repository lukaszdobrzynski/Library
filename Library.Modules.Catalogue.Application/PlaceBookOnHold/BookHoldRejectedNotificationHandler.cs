using Library.BuildingBlocks.EventBus;
using Library.Modules.Catalogue.IntegrationEvents;
using MediatR;

namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class BookHoldRejectedNotificationHandler : INotificationHandler<BookHoldRejectedNotification>
{
    private readonly IEventBus _eventBus;

    public BookHoldRejectedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public async Task Handle(BookHoldRejectedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new BookHoldRejectedIntegrationEvent
        {
            Id = notification.Id,
            OccurredOn = notification.OccurredOn
        });
    }
}