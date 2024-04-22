using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class LibraryManagedHoldStatus : ValueObject
{
    private string Value { get; }
    
    public const string Rejected = nameof(Rejected);
    public const string Cancelled = nameof(Cancelled);
    public const string ReadyToPick = nameof(ReadyToPick);
    public const string Picked = nameof(Picked);
    public const string Granted = nameof(Granted);
    
    private LibraryManagedHoldStatus(string value)
    {
        Value = value;
    }
    
    public static implicit operator LibraryManagedHoldStatus(string value) => new(value);

    public static implicit operator string(LibraryManagedHoldStatus status) => status.Value;
}