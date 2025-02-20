﻿using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.PlaceBookOnHold;

public class BookHoldRejectedNotification : IDomainNotification
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid LibraryBranchId { get; set; }
    public Guid PatronId { get; set; }
    public DateTime OccurredOn { get; set; }

    public Guid ExternalHoldRequestId { get; set; }
}