using NUnit.Framework;

namespace Library.Modules.Reservation.Domain.UnitTests.Holds;

public class CreateHoldTests : HoldTestBase
{ 
    [Test]
    public void Create_Succeeds()
    {
        var hold = CreatePendingHold();
        
        Assert.NotNull(hold);
        AssertHoldActive(hold);
        AssertHoldStatusPending(hold);
    }
}