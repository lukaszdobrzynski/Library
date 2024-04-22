using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class PatronManagedHoldStatus : ValueObject
{
    private string Value { get; }

    public const string Pending = nameof(Pending);
    public const string Cancelled = nameof(Cancelled);
    
    private PatronManagedHoldStatus(string value)
    {
        Value = value;
    }

    public static implicit operator PatronManagedHoldStatus(string value) => new(value);

    public static implicit operator string(PatronManagedHoldStatus status) => status.Value;
}