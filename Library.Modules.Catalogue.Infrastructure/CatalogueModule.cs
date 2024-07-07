using Autofac;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Configuration;
using MediatR;

namespace Library.Modules.Catalogue.Infrastructure;

public class CatalogueModule : ICatalogueModule
{
    public async Task ExecuteCommandAsync(IRequest command)
    {
        using (var scope = CatalogueCompositionRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }

    public async Task<TResult> ExecuteCommandAsync<TResult>(IRequest<TResult> command)
    {
        using (var scope = CatalogueCompositionRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using (var scope = CatalogueCompositionRoot.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(query);
        }
    }
}