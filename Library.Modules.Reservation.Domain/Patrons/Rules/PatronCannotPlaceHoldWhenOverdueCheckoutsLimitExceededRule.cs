using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule : IBusinessRule
    {
        public const int MaxAllowedOverdueCheckouts = 2;
    
        private readonly List<OverdueCheckout> _overdueCheckouts;
    
        public PatronCannotPlaceHoldWhenOverdueCheckoutsLimitExceededRule(List<OverdueCheckout> overdueCheckouts)
    {
        _overdueCheckouts = overdueCheckouts;
    }
    
        public bool IsBroken()
    {
        return _overdueCheckouts.Count >= MaxAllowedOverdueCheckouts;
    }

        public string Message => "Patron has reached overdue checkout limit.";
    }
}