using Autofac;
using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public class DataAccessModule : Autofac.Module
{
    private readonly RavenDatabaseSettings _ravenDatabaseSettings; 
    
    public DataAccessModule(RavenDatabaseSettings ravenDatabaseSettings)
    {
        _ravenDatabaseSettings = ravenDatabaseSettings;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        var documentStoreHolder = new DocumentStoreHolder(_ravenDatabaseSettings);
        
        builder.RegisterInstance(documentStoreHolder)
            .As<IDocumentStoreHolder>()
            .SingleInstance();
    }
}