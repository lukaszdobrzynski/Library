using Autofac;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Queries;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public class DataAccessModule : Autofac.Module
{
    private readonly RavenSettings _ravenSettings; 
    
    public DataAccessModule(RavenSettings ravenSettings)
    {
        _ravenSettings = ravenSettings;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        var documentStoreHolder = new DocumentStoreHolder(_ravenSettings);
        
        builder.RegisterInstance(documentStoreHolder)
            .As<IDocumentStoreHolder>()
            .SingleInstance();

        builder.RegisterType<BookQueries>()
            .As<IBookQueries>()
            .SingleInstance();
    }
}