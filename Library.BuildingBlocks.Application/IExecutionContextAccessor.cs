namespace Library.BuildingBlocks.Application;

public interface IExecutionContextAccessor
{ 
    Guid CorrelationId { get; }
    bool IsAvailable { get; }
}