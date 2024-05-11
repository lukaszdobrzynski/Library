using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Patrons;

public class PatronType : ValueObject
{
    public string Type { get; }

    private PatronType(string value)
    {
        Type = value;
    }

    private PatronType()
    {
        //EF Core only
    }

public static PatronType Regular => new (nameof(Regular));
    public static PatronType Researcher => new(nameof(Researcher));
}