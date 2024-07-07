using MediatR;

namespace Library.Modules.Catalogue.Application.Contracts;

public interface ICatalogueModule
{
    Task ExecuteCommandAsync(IRequest command);

    Task<TResult> ExecuteCommandAsync<TResult>(IRequest<TResult> command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}