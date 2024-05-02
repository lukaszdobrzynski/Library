using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotGrantHoldWhenHoldGrantedRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotGrantHoldWhenHoldGrantedRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.Granted;
    public string Message => "Cannot grant hold when hold is granted";
}