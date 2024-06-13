namespace Library.Modules.Catalogue.Application.Contracts;

public class InvalidCommandTypeException : Exception
{
    public InvalidCommandTypeException(string message) : base(message)
    {
    }
}