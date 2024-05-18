using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Books;

public class BookCategory : ValueObject
{
    public string Value { get; }
    public static BookCategory Circulating => new (nameof(Circulating));
    public static BookCategory Restricted => new(nameof(Restricted));
    
    private BookCategory(string value)
    {
        Value = value;
    }
}