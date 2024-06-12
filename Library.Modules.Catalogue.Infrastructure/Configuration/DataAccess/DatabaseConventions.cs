using Library.Modules.Catalogue.Application.Contracts;
using Raven.Client.Documents.Conventions;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public static class DatabaseConventions
{
    public static void SetUp(DocumentConventions conventions)
    {
        ConfigureCollections(conventions);
    }

    private static void ConfigureCollections(DocumentConventions conventions)
    {
        var defaultFindCollectionName = conventions.FindCollectionName;

        conventions.FindCollectionName = type =>
        {
            if (typeof(InternalCommandBase).IsAssignableFrom(type))
                return "InternalCommands";

            return defaultFindCollectionName(type);
        };
    }
}