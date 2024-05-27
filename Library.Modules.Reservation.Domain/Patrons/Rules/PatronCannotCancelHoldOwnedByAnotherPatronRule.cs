using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class PatronCannotCancelHoldOwnedByAnotherPatronRule : IBusinessRule
    {
        private readonly PatronId _patronId;
        private readonly HoldToCancel _holdToCancel;

        public PatronCannotCancelHoldOwnedByAnotherPatronRule(PatronId patronId, HoldToCancel holdToCancel)
        {
            _patronId = patronId;
            _holdToCancel = holdToCancel;
        }
    
        public bool IsBroken()
        {
            return _patronId != _holdToCancel.OwningPatronId;
        }

        public string Message => "Patron cannot cancel a hold owned by another patron.";
    }
}