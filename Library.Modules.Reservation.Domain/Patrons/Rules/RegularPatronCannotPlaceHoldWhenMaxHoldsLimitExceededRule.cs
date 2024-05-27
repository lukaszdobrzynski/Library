using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule : IBusinessRule
    {
        public const int MaxAllowedHolds = 5;

        private readonly PatronType _patronType;
        private readonly WeeklyActiveHolds _weeklyActiveHolds;
    
        public RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(PatronType patronType, WeeklyActiveHolds weeklyActiveHolds)
        {
            _patronType = patronType;
            _weeklyActiveHolds = weeklyActiveHolds;
        }
    
        public bool IsBroken() => _patronType == PatronType.Regular && _weeklyActiveHolds.Count >= MaxAllowedHolds;
        public string Message => "Patron has reached maximum number of holds.";
    }
}