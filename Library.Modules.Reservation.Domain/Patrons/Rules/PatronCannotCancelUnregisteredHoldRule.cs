using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class PatronCannotCancelNonExistingHoldRule : IBusinessRule
    {
        private readonly Hold _hold;
    
        public PatronCannotCancelNonExistingHoldRule(Hold hold)
        {
            _hold = hold;
        }

        public bool IsBroken() => _hold is null;

        public string Message => "Patron cannot cancel non-existing hold.";
    }
}