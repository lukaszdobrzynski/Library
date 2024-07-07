using MediatR;

namespace Library.Modules.Catalogue.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}