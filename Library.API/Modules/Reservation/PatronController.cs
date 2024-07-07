﻿using System;
using System.Threading.Tasks;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Application.Patrons.CancelHold;
using Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;
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

    [HttpPost]
    [Route("place-book-on-hold")]
    public async Task<IActionResult> PlaceBookOnHold()
    {
        await _reservationModule.ExecuteCommandAsync(new PlaceBookOnHoldCommand(Guid.Parse("f0a18c61-8e49-49d5-a2d8-4c3b6d0a1e3b"), Guid.Parse("f0e4109e-4975-439a-aa11-d369f8d6a9ec")));

        return Ok();
    }

    [HttpPost]
    [Route("cancel-hold")]
    public async Task<IActionResult> CancelHold()
    {
        await _reservationModule.ExecuteCommandAsync(new CancelHoldCommand(Guid.Parse("f0e4109e-4975-439a-aa11-d369f8d6a9ec"), Guid.Parse("310c5732-cc46-44a7-bf40-efec118a0c13")));

        return Ok();
    }
}