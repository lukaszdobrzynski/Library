namespace Library.Modules.Reservation.Infrastructure.DailySheet;

public class HoldDto
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public Guid PatronId { get; set; }

    public Guid LibraryBranchId { get; set; }

    public DateTime Till { get; set; }
}