using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule : IBusinessRule
    {
        public const int MaxAllowedHolds = 5; 
    
        private readonly List<ActiveHold> _existingHolds;
    
        public RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(List<ActiveHold> existingHolds)
    {
        _existingHolds = existingHolds;
    }
    
        public bool IsBroken()
    {
        return _existingHolds.Count >= MaxAllowedHolds;
    }

        public string Message => "Patron has reached maximum number of holds.";
    }
}