using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class LibraryHoldDecisionStatus : ValueObject
{
    private string Value { get; }

    public const string NoDecision = nameof(NoDecision);
    public const string Cancelled = nameof(Cancelled);
    public const string Granted = nameof(Granted);
    public const string Loaned = nameof(Loaned);
    public const string Rejected = nameof(Rejected);
    public const string ReadyToPick = nameof(ReadyToPick);
    
    private LibraryHoldDecisionStatus(string value)
    {
        Value = value;
    }
    
    public static implicit operator LibraryHoldDecisionStatus(string value) => new(value);
    public static implicit operator string(LibraryHoldDecisionStatus decisionStatus) => decisionStatus.Value;
}