﻿using System;
using System.Threading.Tasks;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Application.Patrons;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Modules.Reservation;

[Route("api/reservation/patron")]
[ApiController]
public class PatronController : ControllerBase
{
    private readonly IReservationModule _reservationModule;
    
    public PatronController(IReservationModule reservationModule)
    {
        _reservationModule = reservationModule;
    }

    [HttpPost("place-book-on-hold")]
    public async Task<IActionResult> PlaceBookOnHold()
    {
        await _reservationModule.ExecuteCommandAsync(new PlaceBookOnHoldCommand(Guid.NewGuid(), Guid.Parse("5130ebf9-1aa9-4c8b-8dc5-c804282f26ef")));

        return Ok();
    }

    [HttpPost("cancel-hold")]
    public async Task<IActionResult> CancelHold()
    {
        await _reservationModule.ExecuteCommandAsync(new CancelHoldCommand(Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid()));

        return Ok();
    }
}