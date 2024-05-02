using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotGrantHoldWhenHoldReadyToPickRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotGrantHoldWhenHoldReadyToPickRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.ReadyToPick;
    public string Message => "Cannot grant hold when hold ready to pick.";
}