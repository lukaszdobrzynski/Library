using Autofac;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Queries;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public class DataAccessModule : Autofac.Module
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public DataAccessModule(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_documentStoreHolder)
            .As<IDocumentStoreHolder>()
            .SingleInstance();

        builder.RegisterType<BookQueries>()
            .As<IBookQueries>()
            .SingleInstance();
    }
}