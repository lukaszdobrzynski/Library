using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class PatronHoldDecisionStatus : ValueObject
{
    public string Value { get; }

    public const string NoDecision = nameof(NoDecision);
    public const string Cancelled = nameof(Cancelled);

    private PatronHoldDecisionStatus()
    {
        //EF only
    }
    
    private PatronHoldDecisionStatus(string value)
    {
        Value = value;
    }

    public static implicit operator PatronHoldDecisionStatus(string value) => new(value);
    public static implicit operator string(PatronHoldDecisionStatus decisionStatus) => decisionStatus.Value;
}