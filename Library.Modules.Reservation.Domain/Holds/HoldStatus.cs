using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class HoldStatus : ValueObject
{
    private string Value { get; set; }

    public const string Pending = nameof(Pending);
    public const string Granted = nameof(Granted);
    public const string Rejected = nameof(Rejected);
    public const string Cancelled = nameof(Cancelled);
    public const string ReadyToPick = nameof(ReadyToPick);
    public const string Picked = nameof(Picked);
    
    private HoldStatus(string value)
    {
        Value = value;
    }
    
    public static HoldStatus From(PatronManagedHoldStatus status)
    {
        switch (status)
        {
            case PatronManagedHoldStatus.Cancelled:
                return Cancelled;
            case PatronManagedHoldStatus.Pending:
                return Pending;
            default:
                throw new ArgumentException();
        }
    }

    public static HoldStatus From(LibraryManagedHoldStatus status)
    {
        switch (status)
        {
            case LibraryManagedHoldStatus.Granted:
                return Granted;
            case LibraryManagedHoldStatus.Cancelled:
                return Cancelled;
            case LibraryManagedHoldStatus.Rejected:
                return Rejected;
            case LibraryManagedHoldStatus.ReadyToPick:
                return ReadyToPick;
            case LibraryManagedHoldStatus.Picked:
                return Picked;
            default:
                throw new ArgumentException();
        }
    }

    public static implicit operator string(HoldStatus status) => status.Value;

    public static implicit operator HoldStatus(string value) => new(value);
}