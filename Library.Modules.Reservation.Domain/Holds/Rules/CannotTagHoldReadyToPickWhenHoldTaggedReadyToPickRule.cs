using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotTagHoldReadyToPickWhenHoldTaggedReadyToPickRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotTagHoldReadyToPickWhenHoldTaggedReadyToPickRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.ReadyToPick;
    public string Message => "Cannot tag hold ready to pick when hold is already tagged ready to pick.";
}