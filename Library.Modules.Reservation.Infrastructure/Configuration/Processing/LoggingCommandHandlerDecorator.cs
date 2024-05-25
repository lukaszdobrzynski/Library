using Library.Modules.Reservation.Application.Contracts;
using MediatR;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class LoggingCommandHandlerDecorator<T> : IRequestHandler<T>
    where T : ICommand
{
    private readonly IRequestHandler<T> _decorated;
    private readonly ILogger _logger;
    
    public LoggingCommandHandlerDecorator(IRequestHandler<T> decorated, ILogger logger)
    {
        _decorated = decorated;
        _logger = logger;
    }
    
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        _logger.Information($"Processing {command.GetType().Name}");
        
        await _decorated.Handle(command, cancellationToken);
        
        _logger.Information($"Successfully processed {command.GetType().Name}.");
    }
}