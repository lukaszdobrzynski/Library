using Library.Modules.Catalogue.IntegrationEvents;
using Library.Modules.Reservation.Application.Contracts;
using MediatR;

namespace Library.Modules.Reservation.Application.Holds.RejectHold;

public class BookHoldRejectedIntegrationEventHandler : INotificationHandler<BookHoldRejectedIntegrationEvent>
{
    private readonly IInternalCommandsScheduler _internalCommandsScheduler;

    public BookHoldRejectedIntegrationEventHandler(IInternalCommandsScheduler internalCommandsScheduler)
    {
        _internalCommandsScheduler = internalCommandsScheduler;
    }
    
    public async Task Handle(BookHoldRejectedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _internalCommandsScheduler.Submit(new ApplyRejectHoldDecisionCommand
        {
            Id = Guid.NewGuid(),
            RequestHoldId = notification.RequestHoldId
        });
    }
}