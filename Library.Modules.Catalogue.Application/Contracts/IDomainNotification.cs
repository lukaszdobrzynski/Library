using MediatR;

namespace Library.Modules.Catalogue.Application.Contracts;

public interface IDomainNotification : INotification
{
    Guid Id { get; set; }
}