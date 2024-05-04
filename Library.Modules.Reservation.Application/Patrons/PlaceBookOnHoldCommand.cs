using Library.Modules.Reservation.Application.Contracts;

namespace Library.Modules.Reservation.Application.Patrons;

public class PlaceBookOnHoldCommand : CommandBase
{
    public Guid BookId { get; private set; }
    public Guid PatronId { get; private set; }

    public PlaceBookOnHoldCommand(Guid bookId, Guid patronId)
    {
        BookId = bookId;
        PatronId = patronId;
    }
}