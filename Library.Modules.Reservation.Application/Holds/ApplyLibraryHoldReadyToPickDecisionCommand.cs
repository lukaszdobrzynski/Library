﻿using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Holds;

public class ApplyLibraryHoldReadyToPickDecisionCommand(Guid holdId) : CommandBase
{
    public Guid HoldId { get; private set; } = holdId;
}