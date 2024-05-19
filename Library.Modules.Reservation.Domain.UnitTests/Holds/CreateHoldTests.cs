using Library.Modules.Reservation.Domain.Holds.Events;
using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class CreateHoldTests : HoldTestBase
{ 
    [Test]
    public void Create_Succeeds()
    {
        var hold = CreatePendingHold();

        var domainEvent = AssertDomainEventPublished<HoldCreatedDomainEvent>(hold);
        
        Assert.That(domainEvent.HoldId, Is.EqualTo(hold.Id));
    }
}