using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds.Events;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class CancelHoldDecisionAppliedNotification : IDomainEventNotification<CancelHoldDecisionAppliedDomainEvent>
{
    public Guid Id { get; set; }
    public CancelHoldDecisionAppliedDomainEvent DomainEvent { get; }

    public CancelHoldDecisionAppliedNotification(CancelHoldDecisionAppliedDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}