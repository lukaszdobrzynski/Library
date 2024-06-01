using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule : IBusinessRule
    {
        public const int MaxAllowedHolds = 5;

        private readonly PatronType _patronType;
        private readonly ActiveHolds _activeHolds;
    
        public RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(PatronType patronType, ActiveHolds activeHolds)
        {
            _patronType = patronType;
            _activeHolds = activeHolds;
        }
    
        public bool IsBroken() => _patronType == PatronType.Regular && _activeHolds.Count >= MaxAllowedHolds;
        public string Message => "Patron has reached maximum number of holds.";
    }
}