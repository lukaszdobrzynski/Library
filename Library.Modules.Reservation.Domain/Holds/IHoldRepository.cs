﻿using Library.Modules.Reservation.Domain.Patrons;

namespace Library.Modules.Reservation.Domain.Holds;

public interface IHoldRepository
{
    Task<Hold> GetByIdAsync(HoldId holdId);
    Task<List<ActiveHold>> GetActiveHoldsByPatronIdAsync(PatronId patronId);
    Task AddAsync(Hold hold);
}