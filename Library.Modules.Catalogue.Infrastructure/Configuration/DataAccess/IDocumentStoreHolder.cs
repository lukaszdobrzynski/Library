using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public interface IDocumentStoreHolder : IDisposable
{
    IAsyncDocumentSession OpenAsyncSession();
}