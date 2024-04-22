using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class PatronCannotCancelHoldOwnedByAnotherPatronRule : IBusinessRule
    {
        private PatronId _patronId;

        private BookOnHold _bookOnHold;
    
        public PatronCannotCancelHoldOwnedByAnotherPatronRule(PatronId patronId, BookOnHold bookOnHold)
        {
            _patronId = patronId;
            _bookOnHold = bookOnHold;
        }
    
        public bool IsBroken()
        {
            return _patronId != _bookOnHold.PatronId;
        }

        public string Message => "Patron cannot cancel a hold owned by another patron.";
    }
}