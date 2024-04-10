using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Rules;

public class RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule : IBusinessRule
{
    private const int MaxNumberOfHolds = 5; 
    
    private readonly List<Hold> _existingHolds;
    
    public RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(List<Hold> existingHolds)
    {
        _existingHolds = existingHolds;
    }
    
    public bool IsBroken()
    {
        return _existingHolds.Count >= MaxNumberOfHolds;
    }

    public string Message => "Patron has exceeded maximum number of holds.";
}