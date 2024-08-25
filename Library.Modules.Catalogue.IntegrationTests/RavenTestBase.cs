using System;
using System.IO;
using System.Runtime.Loader;
using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.TestDriver;

namespace Library.Modules.Catalogue.IntegrationTests;

public class RavenTestBase : RavenTestDriver
{
    private IDocumentStore _testDriverDocumentStore;
    private DocumentStoreHolder _documentStoreHolder;

    protected RavenTestBase()
    {
        ConfigureServer(new TestServerOptions
        {
            FrameworkVersion = null // use the latest installed framework version
        });

        AssemblyLoadContext.Default.Unloading += c => DeleteTestDriverDatabaseFiles();
    }
    
    protected override void PreInitialize(IDocumentStore documentStore)
    {
        var databaseSettings = new RavenSettings
        {
            Urls = documentStore.Urls,
            DatabaseName = documentStore.Database
        };

        DocumentStoreHolder = new DocumentStoreHolder(databaseSettings);
    }
    
    protected virtual void BeforeEachTest()
    {
        _testDriverDocumentStore = GetDocumentStore(options: null, database: Guid.NewGuid().ToString());
    }

    protected DocumentStoreHolder DocumentStoreHolder
    {
        get
        {
            if (_documentStoreHolder is not null)
                return _documentStoreHolder;
                
            throw new InvalidOperationException($"Test {nameof(DocumentStoreHolder)} is null");
        }
        private set => _documentStoreHolder = value;
    }
    
    protected IDocumentSession OpenSession() => DocumentStoreHolder.RavenDocumentStore.OpenSession();
    protected IAsyncDocumentSession OpenAsyncSession() => DocumentStoreHolder.RavenDocumentStore.OpenAsyncSession();
    
    private static void DeleteTestDriverDatabaseFiles()
    {
        var dbDirectoryPath = Path.Join(".", "RavenDB");
            
        try
        {
            Directory.Delete(dbDirectoryPath, recursive: true);
        }
        catch
        {
            // ignored
        }
    }
    
    protected void WaitForUserToContinueTheTest()
        => WaitForUserToContinueTheTest(DocumentStoreHolder.RavenDocumentStore);
}