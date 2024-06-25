namespace Library.Modules.Reservation.Application.Contracts;

public abstract class InternalCommandBase : ICommand
{
    public Guid Id { get; }
    
}