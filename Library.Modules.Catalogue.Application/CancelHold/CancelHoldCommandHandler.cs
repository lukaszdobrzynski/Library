using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Models;

namespace Library.Modules.Catalogue.Application.CancelHold;

public class CancelHoldCommandHandler : InternalCommandHandler<CancelHoldCommand>
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public CancelHoldCommandHandler(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }
    
    protected override async Task HandleConcreteCommand(CancelHoldCommand command)
    {
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            var book = await session.LoadAsync<Book>(command.BookId.ToString());
            book.Status = BookStatus.Available;
            await session.SaveChangesAsync();
        }
    }
}