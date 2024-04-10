using Library.BuildingBlocks.Domain;

namespace Library.Modules.Lending.Domain.Rules;

public class BookCategoryMustBeDefinedRule : IBusinessRule
{
    private readonly string _value;
    
    public BookCategoryMustBeDefinedRule(string value)
    {
        _value = value;
    }
    
    public bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_value);
    }

    public string Message => "Book category value must be defined.";
}