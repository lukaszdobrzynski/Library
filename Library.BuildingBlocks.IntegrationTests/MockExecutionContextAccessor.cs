using System;
using Library.BuildingBlocks.Application;

namespace Library.BuildingBlocks.IntegrationTests;

public class MockExecutionContextAccessor : IExecutionContextAccessor
{
    public Guid CorrelationId { get; set; }
    public bool IsAvailable { get; set; }
}