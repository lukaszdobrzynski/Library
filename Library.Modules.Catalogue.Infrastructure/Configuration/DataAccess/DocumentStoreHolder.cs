using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Configuration.Processing;
using Library.Modules.Catalogue.Infrastructure.Inbox;
using Library.Modules.Catalogue.Infrastructure.Outbox;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public class DocumentStoreHolder : IDocumentStoreHolder
{
    private readonly IDocumentStore _documentStore;
    
    public DocumentStoreHolder(RavenDatabaseSettings settings)
    {
        ValidateSettings(settings);
        
        _documentStore = CreateDocumentStore(settings);
    }

    private DocumentStore CreateDocumentStore(RavenDatabaseSettings settings)
    {
        var store = new DocumentStore
        {
            Urls = settings.Urls,
            Database = settings.DatabaseName
        };
        
        store.OnBeforeQuery += (sender, beforeQueryExecutedArgs) =>
        {
            beforeQueryExecutedArgs.QueryCustomization.WaitForNonStaleResults();
        };

        if (settings.CertificatePath != null)
        {
            var certificatePassword = settings.CertificatePassword;
            var certificate = new X509Certificate2(settings.CertificatePath, certificatePassword);
            store.Certificate = certificate;
                
            store.AfterDispose += (sender, args) => certificate.Dispose();
        }

        if (settings.RequestTimeoutInSeconds.HasValue)
        {
            store.Conventions.RequestTimeout = TimeSpan.FromSeconds(settings.RequestTimeoutInSeconds.Value);
        }

        store.OnBeforeQuery += (sender, args) =>
            args.QueryCustomization.WaitForNonStaleResults(TimeSpan.FromSeconds(30));
        
        DatabaseConventions.SetUp(store.Conventions);
        
        store.Initialize();
        
#if DEBUG
        CreateSubscriptions(store);
        DeployIndexes(store, settings);
#endif        
        
        return store;
    }

    private static void CreateSubscriptions(IDocumentStore store)
    {
        InboxSubscription.Create(store);
        InternalCommandsSubscription.Create(store);
        OutboxSubscription.Create(store);
    }

    private static void DeployIndexes(IDocumentStore store, RavenDatabaseSettings settings)
    {
        var database = settings.DatabaseName;
        var assembly = Assembly.GetExecutingAssembly();
        IndexCreation.CreateIndexes(assembly, store, database: database);
    }

    private void ValidateSettings(RavenDatabaseSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.DatabaseName))
        {
            throw new DatabaseConfigurationException(
                $"{nameof(RavenDatabaseSettings.DatabaseName)} was not provided in the settings.");
        }

        if (settings.Urls == null || settings.Urls.Length < 1)
        {
            throw new DatabaseConfigurationException($"Database configuration {nameof(RavenDatabaseSettings.Urls)} were not provided in the settings.");
        }
        
#if !DEBUG
        if (string.IsNullOrEmpty(settings.CertificatePath))
                throw new DatabaseConfigurationException($"{nameof(settings.CertificatePath)} was not provided in the settings.");

        if (File.Exists(settings.CertificatePath) == false)
                throw new DatabaseConfigurationException($"File under {nameof(settings.CertificatePath)} does not exist.");
#endif
    }
    
    public IAsyncDocumentSession OpenAsyncSession()
    {
        return _documentStore.OpenAsyncSession();
    }

    public SubscriptionWorker<T> GetSubscriptionWorker<T>(SubscriptionWorkerOptions options) where T : class
    {
        return _documentStore.Subscriptions.GetSubscriptionWorker<T>(options);
    }

    public void Dispose()
    {
        _documentStore?.Dispose();
    }
}