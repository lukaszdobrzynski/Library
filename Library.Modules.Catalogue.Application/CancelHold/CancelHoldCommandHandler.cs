using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.CancelHold;

public class CancelHoldCommandHandler : InternalCommandHandler<CancelHoldCommand>
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public CancelHoldCommandHandler(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }
    
    protected override Task HandleConcreteCommand(CancelHoldCommand command)
    {
        throw new NotImplementedException();
    }
}