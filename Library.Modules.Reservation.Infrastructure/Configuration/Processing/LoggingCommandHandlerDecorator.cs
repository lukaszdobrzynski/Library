using Library.Modules.Reservation.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class LoggingCommandHandlerDecorator<T> : IRequestHandler<T>
    where T : ICommand
{
    private readonly IRequestHandler<T> _handler;
    private readonly ILogger _logger;
    
    public LoggingCommandHandlerDecorator(IRequestHandler<T> handler, ILogger logger)
    {
        _handler = handler;
        _logger = logger;
    }
    
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await _handler.Handle(command, cancellationToken);
    }
}