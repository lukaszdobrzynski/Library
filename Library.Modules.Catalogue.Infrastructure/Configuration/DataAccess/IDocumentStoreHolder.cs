using Raven.Client.Documents.Session;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public interface IDocumentStoreHolder : IDisposable
{
    IAsyncDocumentSession OpenAsyncSession();
    SubscriptionWorker<T> GetSubscriptionWorker<T>(SubscriptionWorkerOptions options) where T : class;
}