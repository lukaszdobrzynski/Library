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

    public const string Regular = nameof(Regular);
    public const string Researcher = nameof(Researcher);
    
    public static implicit operator string(PatronType type) => type.Type;
    public static implicit operator PatronType(string value) => new(value);
}