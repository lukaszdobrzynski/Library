using Library.BuildingBlocks.Application.Outbox;

namespace Library.BuildingBlocks.Infrastructure;

public interface IOutboxAccessor
{
    void Add(OutboxMessage outboxMessage);
}