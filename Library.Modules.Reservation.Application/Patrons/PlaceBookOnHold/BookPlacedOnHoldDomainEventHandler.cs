﻿using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons.Events;
using MediatR;

namespace Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

public class BookPlacedOnHoldDomainEventHandler(IHoldRepository holdRepository) : INotificationHandler<BookPlacedOnHoldDomainEvent>
{
    public async Task Handle(BookPlacedOnHoldDomainEvent @event, CancellationToken cancellationToken)
    {
        var hold = Hold.Create(@event.BookId, @event.LibraryBranchId, @event.PatronId, @event.Till, new HoldRequestId(@event.HoldRequestId.Value));
        await holdRepository.AddAsync(hold);
    }
}