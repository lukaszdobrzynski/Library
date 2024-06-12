namespace Library.Modules.Catalogue.Application.Contracts;

public class InvalidInternalCommandException : Exception
{
    public InvalidInternalCommandException(string message) : base(message)
    {
    }
}