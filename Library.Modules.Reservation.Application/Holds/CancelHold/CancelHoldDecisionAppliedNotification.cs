using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Holds.Events;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class CancelHoldDecisionAppliedNotification : IDomainNotification<CancelHoldDecisionAppliedDomainEvent>
{
    public Guid Id { get; }
    public CancelHoldDecisionAppliedDomainEvent DomainEvent { get; }

    public CancelHoldDecisionAppliedNotification(CancelHoldDecisionAppliedDomainEvent domainEvent, Guid id)
    {
        DomainEvent = domainEvent;
        Id = id;
    }
}