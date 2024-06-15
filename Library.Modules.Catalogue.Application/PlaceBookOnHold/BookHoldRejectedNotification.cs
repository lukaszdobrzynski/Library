namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class BookHoldRejectedNotification
{
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public Guid PatronId { get; set; }
}