namespace Library.BuildingBlocks.Infrastructure;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}