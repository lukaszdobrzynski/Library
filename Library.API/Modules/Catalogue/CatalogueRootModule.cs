using Autofac;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure;

namespace Library.API.Modules.Catalogue;

public class CatalogueRootModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CatalogueModule>()
            .As<ICatalogueModule>()
            .InstancePerLifetimeScope();
    }
}