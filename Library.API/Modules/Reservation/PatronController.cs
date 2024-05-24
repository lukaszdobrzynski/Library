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
        await _reservationModule.ExecuteCommandAsync(new PlaceBookOnHoldCommand(Guid.Parse("0eaf7c86-c22a-4fe1-9508-08d3d52a8c78"), Guid.Parse("f0e4109e-4975-439a-aa11-d369f8d6a9ec")));

        return Ok();
    }

    [HttpPost("cancel-hold")]
    public async Task<IActionResult> CancelHold()
    {
        await _reservationModule.ExecuteCommandAsync(new CancelHoldCommand(Guid.NewGuid(), Guid.NewGuid()));

        return Ok();
    }
}