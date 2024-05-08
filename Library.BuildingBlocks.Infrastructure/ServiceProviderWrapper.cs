using Autofac;

namespace Library.BuildingBlocks.Infrastructure;

public class ServiceProviderWrapper : IServiceProvider
{
    private readonly ILifetimeScope _lifetimeScope;
    
    public ServiceProviderWrapper(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public object? GetService(Type serviceType) => _lifetimeScope.ResolveOptional(serviceType);
}