using Library.Modules.Reservation.Application.Outbox;

namespace Library.Modules.Reservation.Infrastructure.Outbox;

public interface IOutboxAccessor
{
    void Add(OutboxMessage outboxMessage);
}