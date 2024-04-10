using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Book;

namespace Library.Modules.Lending.Domain.Rules;

public class RegularPatronCannotPlaceHoldOnRestrictedBookRule : IBusinessRule
{
    private readonly BookCategory _bookCategory;

    public RegularPatronCannotPlaceHoldOnRestrictedBookRule(BookCategory bookCategory)
    {
        _bookCategory = bookCategory;
    }

    public bool IsBroken() => _bookCategory == BookCategory.Restricted;

    public string Message => "Regular patron cannot place a hold on a restricted book.";
}