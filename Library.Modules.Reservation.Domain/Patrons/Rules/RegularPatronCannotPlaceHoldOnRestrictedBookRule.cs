using Library.BuildingBlocks.Domain;
using Library.Modules.Reservation.Domain.Books;

namespace Library.Modules.Reservation.Domain.Patrons.Rules
{
    public class RegularPatronCannotPlaceHoldOnRestrictedBookRule : IBusinessRule
    {
        private readonly PatronType _patronType;
        private readonly BookCategory _bookCategory;

        public RegularPatronCannotPlaceHoldOnRestrictedBookRule(PatronType patronType, BookCategory bookCategory)
        {
            _patronType = patronType;
            _bookCategory = bookCategory;
        }

        public bool IsBroken() => _patronType == PatronType.Regular && _bookCategory == BookCategory.Restricted;

        public string Message => "Regular patron cannot place a hold on a restricted book.";
    }
}