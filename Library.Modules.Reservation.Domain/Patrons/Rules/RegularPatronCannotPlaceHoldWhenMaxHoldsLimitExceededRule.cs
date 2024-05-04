using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule : IBusinessRule
    {
        public const int MaxAllowedHolds = 5;

        private readonly PatronType _patronType;
        private readonly List<ActiveHold> _existingHolds;
    
        public RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(PatronType patronType, List<ActiveHold> existingHolds)
        {
            _patronType = patronType;
            _existingHolds = existingHolds;
        }
    
        public bool IsBroken() => _patronType == PatronType.Regular && _existingHolds.Count >= MaxAllowedHolds;
        public string Message => "Patron has reached maximum number of holds.";
    }
}