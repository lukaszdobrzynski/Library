using Library.Modules.Catalogue.IntegrationEvents;
using Library.Modules.Reservation.Application.Contracts;
using MediatR;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class BookHoldGrantedIntegrationEventHandler : INotificationHandler<BookHoldGrantedIntegrationEvent>
{
    private readonly IInternalCommandsScheduler _internalCommandsScheduler;
    
    public BookHoldGrantedIntegrationEventHandler(IInternalCommandsScheduler internalCommandsScheduler)
    {
        _internalCommandsScheduler = internalCommandsScheduler;
    }
    
    public async Task Handle(BookHoldGrantedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _internalCommandsScheduler.Submit(new ApplyGrantHoldDecisionCommand(Guid.NewGuid()));
    }
}