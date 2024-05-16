using System;
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
        await _reservationModule.ExecuteCommandAsync(new PlaceBookOnHoldCommand(Guid.NewGuid(), Guid.Parse("aa4f2581-9307-4d4f-b2fe-e7afb13a78ba")));

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