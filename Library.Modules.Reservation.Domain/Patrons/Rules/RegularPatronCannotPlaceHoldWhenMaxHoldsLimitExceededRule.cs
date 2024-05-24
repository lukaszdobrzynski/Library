using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule : IBusinessRule
    {
        public const int MaxAllowedHolds = 5;

        private readonly PatronType _patronType;
        private readonly WeeklyHolds _weeklyHolds;
    
        public RegularPatronCannotPlaceHoldWhenMaxHoldsLimitExceededRule(PatronType patronType, WeeklyHolds weeklyHolds)
        {
            _patronType = patronType;
            _weeklyHolds = weeklyHolds;
        }
    
        public bool IsBroken() => _patronType == PatronType.Regular && _weeklyHolds.Holds.Count >= MaxAllowedHolds;
        public string Message => "Patron has reached maximum number of holds.";
    }
}