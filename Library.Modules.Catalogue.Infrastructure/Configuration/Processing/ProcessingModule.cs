using Autofac;
using Library.BuildingBlocks.Infrastructure;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Processing;

public class ProcessingModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CatalogueRetryPolicyFactory>()
            .As<IRetryPolicyFactory>()
            .InstancePerLifetimeScope();
    }
}