﻿using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Patrons.Events;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class HoldCanceledNotification : IDomainNotification<HoldCanceledDomainEvent>
{
    public Guid Id { get; }
    public HoldCanceledDomainEvent DomainEvent { get; }

    public HoldCanceledNotification(HoldCanceledDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}