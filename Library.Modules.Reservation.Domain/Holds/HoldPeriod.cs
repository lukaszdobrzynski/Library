using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds;

public class HoldPeriod : ValueObject
{
    public string Value { get; }

    public HoldPeriod(string value)
    {
        Value = value;
    }

    public const string Unlimited = nameof(Unlimited);
    public const string Weekly = nameof(Weekly);
    
    public static implicit operator string(HoldPeriod period) => period.Value;
    public static implicit operator HoldPeriod(string value) => new(value);
}