using FluentValidation;
using Library.BuildingBlocks.Application;
using Library.Modules.Reservation.Application.Contracts;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

internal class ValidationCommandHandlerDecorator<T> : IRequestHandler<T>
    where T : ICommand
{
    private readonly IRequestHandler<T> _handler;
    private readonly IValidator<T> _validator;
    
    public ValidationCommandHandlerDecorator(IRequestHandler<T> handler, IValidator<T> validator)
    {
        _handler = handler;
        _validator = validator;
    }
    
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command, cancellationToken);

        if (result.Errors.Any())
        {
            var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new InvalidCommandException(errors);
        }

        await _handler.Handle(command, cancellationToken);
    }
}