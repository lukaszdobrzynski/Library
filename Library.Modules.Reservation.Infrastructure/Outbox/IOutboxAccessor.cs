using Library.BuildingBlocks.Application.Outbox;

namespace Library.Modules.Reservation.Infrastructure;

public interface IOutboxAccessor
{
    void Add(OutboxMessage outboxMessage);
}