using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class PatronCannotCancelHoldOwnedByAnotherPatronRule : IBusinessRule
    {
        private PatronId _patronId;

        private Hold _hold;
    
        public PatronCannotCancelHoldOwnedByAnotherPatronRule(PatronId patronId, Hold hold)
        {
            _patronId = patronId;
            _hold = hold;
        }
    
        public bool IsBroken()
        {
            return _patronId != _hold.PatronId;
        }

        public string Message => "Patron cannot cancel a hold owned by another patron.";
    }
}