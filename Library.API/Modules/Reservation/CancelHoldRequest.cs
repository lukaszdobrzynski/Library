using System;
using System.ComponentModel.DataAnnotations;

namespace Library.API.Modules.Reservation;

public class CancelHoldRequest
{
    [Required]
    public Guid? HoldId { get; set; }
    
    [Required]
    public Guid? PatronId { get; set; }
}