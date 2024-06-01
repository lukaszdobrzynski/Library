using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class ActiveHoldsCountCannotBeNegative : IBusinessRule
{
    private readonly int _count;
    
    public ActiveHoldsCountCannotBeNegative(int count)
    {
        _count = count;
    }

    public bool IsBroken() => _count < 0;

    public string Message => "Active holds count cannot be negative.";
}