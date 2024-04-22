using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Holds;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class PatronCannotCancelUnregisteredHoldRule : IBusinessRule
    {
        private BookOnHold _bookOnHold;
        private List<ActiveHold> _patronHolds;
    
        public PatronCannotCancelUnregisteredHoldRule(BookOnHold bookOnHold, List<ActiveHold> patronHolds)
    {
        _bookOnHold = bookOnHold;
        _patronHolds = patronHolds;
    }

        public bool IsBroken() => 
            _patronHolds.Select(x => x.BookId).Contains(_bookOnHold.BookId) == false;

        public string Message => "Patron cannot cancel unregistered hold.";
    }
}