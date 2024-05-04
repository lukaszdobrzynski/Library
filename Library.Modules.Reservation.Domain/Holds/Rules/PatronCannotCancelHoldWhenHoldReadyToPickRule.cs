using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class PatronCannotCancelHoldWhenHoldReadyToPickRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public PatronCannotCancelHoldWhenHoldReadyToPickRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }

    public bool IsBroken() => _holdStatus == HoldStatus.ReadyToPick;
    public string Message => "Patron cannot cancel a ready to pick rule";
}