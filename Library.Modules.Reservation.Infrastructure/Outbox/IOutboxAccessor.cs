using Library.Modules.Reservation.Application.Outbox;

namespace Library.Modules.Reservation.Infrastructure.Outbox;

internal interface IOutboxAccessor
{
    void Add(OutboxMessage outboxMessage);
}