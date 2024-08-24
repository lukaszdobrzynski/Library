using System.Collections.Generic;
using Serilog;
using Serilog.Events;

namespace Library.BuildingBlocks.IntegrationTests;

public class MockLogger : ILogger
{
    private readonly List<LogEvent> _logs = new();
    
    public void Write(LogEvent logEvent)
    {
        _logs.Add(logEvent);
    }
}