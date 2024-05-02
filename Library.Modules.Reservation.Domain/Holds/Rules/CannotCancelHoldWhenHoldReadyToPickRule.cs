using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotCancelHoldWhenHoldReadyToPickRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotCancelHoldWhenHoldReadyToPickRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.ReadyToPick;
    public string Message => "Cannot cancel hold when hold is ready to pick.";
}