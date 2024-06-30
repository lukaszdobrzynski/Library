using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.RestoreBookAvailability;

public class BookMadeAvailableForReservationNotification : IDomainNotification
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
}