using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Patrons;

public class PatronType : ValueObject
{
    private string _value;
    
    private PatronType(string value)
    {
        _value = value;
    }

    public static PatronType Regular => new (nameof(Regular));
    public static PatronType Researcher => new(nameof(Researcher));
}