using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class WeeklyActiveHoldsCountCannotBeNegative : IBusinessRule
{
    private readonly int _count;
    
    public WeeklyActiveHoldsCountCannotBeNegative(int count)
    {
        _count = count;
    }

    public bool IsBroken() => _count < 0;

    public string Message => "Weekly active holds count cannot be negative";
}