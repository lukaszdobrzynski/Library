using Library.BuildingBlocks.Application;
using Library.Modules.Reservation.Application.Contracts;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class LoggingCommandHandlerDecorator<T> : IRequestHandler<T>
    where T : ICommand
{
    private readonly IRequestHandler<T> _decorated;
    private readonly ILogger _logger;
    private readonly IExecutionContextAccessor _executionContextAccessor;
    
    public LoggingCommandHandlerDecorator(IRequestHandler<T> decorated, ILogger logger, IExecutionContextAccessor executionContextAccessor)
    {
        _decorated = decorated;
        _logger = logger;
        _executionContextAccessor = executionContextAccessor;
    }
    
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        using (LogContext.Push(
                   new RequestLogEnricher(_executionContextAccessor), 
                   new CommandLogEnricher(command)))
        {
            _logger.Information($"Processing {command.GetType().Name}");
        
            await _decorated.Handle(command, cancellationToken);
        
            _logger.Information($"Successfully processed {command.GetType().Name}.");    
        }
    }

    private class CommandLogEnricher : ILogEventEnricher
    {
        private readonly T _command;
        
        public CommandLogEnricher(T command)
        {
            _command = command;
        }
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("CommandId", new ScalarValue(_command.Id)));
        }
    }

    private class RequestLogEnricher : ILogEventEnricher
    {
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
        {
            _executionContextAccessor = executionContextAccessor;
        }
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_executionContextAccessor.IsAvailable)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_executionContextAccessor.CorrelationId)));
            }
        }
    }
}