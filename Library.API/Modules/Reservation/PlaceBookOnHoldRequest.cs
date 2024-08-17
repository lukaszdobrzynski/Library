using System;
using System.ComponentModel.DataAnnotations;

namespace Library.API.Modules.Reservation;

public class PlaceBookOnHoldRequest
{
    [Required]
    public Guid? BookId { get; set; }
    
    [Required]
    public Guid? PatronId { get; set; }
}