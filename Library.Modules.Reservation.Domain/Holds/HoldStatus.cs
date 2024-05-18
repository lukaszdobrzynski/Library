using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class HoldStatus : ValueObject
{
    public string Value { get; }

    public const string Pending = nameof(Pending);
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

    public static HoldStatus From(PatronHoldDecisionStatus patronHoldDecisionStatus,
        LibraryHoldDecisionStatus libraryHoldDecisionStatus)
    {
        switch (patronHoldDecisionStatus)
        {
            case PatronHoldDecisionStatus.NoDecision
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.NoDecision:
                return Pending;
            case PatronHoldDecisionStatus.NoDecision
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.Granted:
                return Granted;
            case PatronHoldDecisionStatus.NoDecision
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.Rejected:
                return Rejected;
            case PatronHoldDecisionStatus.NoDecision
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.Cancelled:
                return Cancelled;
            case PatronHoldDecisionStatus.NoDecision
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.ReadyToPick:
                return ReadyToPick;
            case PatronHoldDecisionStatus.NoDecision
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.Loaned:
                return Loaned;
            case PatronHoldDecisionStatus.Cancelled
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.NoDecision:
                return Cancelled;
            case PatronHoldDecisionStatus.Cancelled
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.Granted:
                return Cancelled;
            case PatronHoldDecisionStatus.Cancelled
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.Rejected:
                return Rejected;
            case PatronHoldDecisionStatus.Cancelled
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.Cancelled:
                return Cancelled;
            case PatronHoldDecisionStatus.Cancelled
                when libraryHoldDecisionStatus == LibraryHoldDecisionStatus.ReadyToPick:
                return ReadyToPick;
            default:
                throw new ArgumentException($"Unrecognized {nameof(PatronHoldDecisionStatus)} and {nameof(libraryHoldDecisionStatus)} mapping.");
        }
    }
}