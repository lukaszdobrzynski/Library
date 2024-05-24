using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class HoldStatus : ValueObject
{
    public string Value { get; }

    public const string PendingConfirmation = nameof(PendingConfirmation);
    public const string Cancelled = nameof(Cancelled);
    public const string Granted = nameof(Granted);
    public const string Rejected = nameof(Rejected);
    public const string Loaned = nameof(Loaned);
    public const string ReadyToPick = nameof(ReadyToPick);

    private HoldStatus(string value)
    {
        Value = value;
    }

    public static implicit operator string(HoldStatus status) => status.Value;
    public static implicit operator HoldStatus(string value) => new(value);
}