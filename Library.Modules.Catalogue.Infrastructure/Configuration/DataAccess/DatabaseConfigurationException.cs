namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public class DatabaseConfigurationException : Exception
{
    public DatabaseConfigurationException(string message) : base(message)
    {
    }
}