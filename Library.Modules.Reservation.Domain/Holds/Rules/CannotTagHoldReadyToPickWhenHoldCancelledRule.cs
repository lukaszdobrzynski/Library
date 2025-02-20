﻿using Library.BuildingBlocks.Domain;

namespace Library.Modules.Reservation.Domain.Holds.Rules;

public class CannotTagHoldReadyToPickWhenHoldCancelledRule : IBusinessRule
{
    private readonly HoldStatus _holdStatus;

    public CannotTagHoldReadyToPickWhenHoldCancelledRule(HoldStatus holdStatus)
    {
        _holdStatus = holdStatus;
    }
    
    public bool IsBroken() => _holdStatus == HoldStatus.Cancelled;
    public string Message => "Cannot tag hold ready to pick when hold is cancelled.";
}