using Library.Modules.Reservation.Application.Outbox;

namespace Library.Modules.Reservation.Infrastructure.Outbox;

public class OutboxAccessor : IOutboxAccessor
{
    private readonly ReservationContext _reservationContext;
    
    public OutboxAccessor(ReservationContext reservationContext)
    {
        _reservationContext = reservationContext;
    }

    public void Add(OutboxMessage outboxMessage)
    {
        _reservationContext.OutboxMessages.Add(outboxMessage);
    }
}