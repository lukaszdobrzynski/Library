namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class BookHoldGrantedNotification
{
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public Guid PatronId { get; set; }
}