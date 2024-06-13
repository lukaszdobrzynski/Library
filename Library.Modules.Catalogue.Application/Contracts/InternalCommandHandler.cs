namespace Library.Modules.Catalogue.Application.Contracts;

public abstract class InternalCommandHandler<TCommand> : InternalCommandHandler 
    where TCommand : InternalCommandBase 
{
    public override async Task Handle(InternalCommandBase command)
    {
        if (command is not TCommand convertedCommand)
        {
            throw new InvalidCommandTypeException($"Could not convert the command with ID {command.Id} to type {typeof(TCommand)}");
        }
        
        command.Validate();

        await HandleConcreteCommand(convertedCommand);
    }

    protected abstract Task HandleConcreteCommand(TCommand command);
}

public abstract class InternalCommandHandler
{
    public abstract Task Handle(InternalCommandBase command);
}
