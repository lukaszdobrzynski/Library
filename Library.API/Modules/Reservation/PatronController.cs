using System;
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
    public async Task<IActionResult> PlaceBookOnHold(PlaceBookOnHoldRequest request)
    {
        await _reservationModule.ExecuteCommandAsync(new PlaceBookOnHoldCommand(request.BookId.Value, request.PatronId.Value));

        return Ok();
    }

    [HttpPost]
    [Route("cancel-hold")]
    public async Task<IActionResult> CancelHold(CancelHoldRequest request)
    {
        await _reservationModule.ExecuteCommandAsync(new CancelHoldCommand(request.PatronId.Value, request.HoldId.Value));

        return Ok();
    }
}