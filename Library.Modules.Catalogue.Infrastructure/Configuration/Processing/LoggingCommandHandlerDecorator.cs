using Library.BuildingBlocks.Application;
using Library.Modules.Catalogue.Application.Contracts;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Processing;

public class LoggingCommandHandlerDecorator<TCommand> : InternalCommandHandler<TCommand>
    where TCommand : InternalCommandBase
{
    private readonly InternalCommandHandler<TCommand> _decorated;
    private readonly ILogger _logger;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public LoggingCommandHandlerDecorator(InternalCommandHandler<TCommand> decorated, 
        ILogger logger, 
        IExecutionContextAccessor executionContextAccessor)
    {
        _decorated = decorated;
        _logger = logger;
        _executionContextAccessor = executionContextAccessor;
    }
    
    protected override async Task HandleConcreteCommand(TCommand command)
    {
        using (LogContext.Push(
                   new RequestLogEnricher(_executionContextAccessor), 
                   new CommandLogEnricher(command)))
        {
            _logger.Information($"Processing {command.GetType().Name}");
        
            await _decorated.Handle(command);
        
            _logger.Information($"Successfully processed {command.GetType().Name}.");    
        }
    }
    
    private class CommandLogEnricher : ILogEventEnricher
    {
        private readonly TCommand _command;
        
        public CommandLogEnricher(TCommand command)
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