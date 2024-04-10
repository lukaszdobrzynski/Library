using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Rules;

public class PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule : IBusinessRule
{
    private const int MaxNumberOfCheckouts = 2;
    
    private readonly List<OverdueCheckout> _overdueCheckouts;
    
    public PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule(List<OverdueCheckout> overdueCheckouts)
    {
        _overdueCheckouts = overdueCheckouts;
    }
    
    public bool IsBroken()
    {
        return _overdueCheckouts.Count >= MaxNumberOfCheckouts;
    }

    public string Message => "Patron has exceeded overdue checkout limit.";
}