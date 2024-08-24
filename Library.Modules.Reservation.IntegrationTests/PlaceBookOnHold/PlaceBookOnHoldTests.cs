using System.Threading.Tasks;
using Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Patrons;
using NUnit.Framework;

namespace Library.Modules.Reservation.IntegrationTests.PlaceBookOnHold;

[SetUpFixture]
public class TestFixtureSetup : TestFixtureSetupBase
{
    [OneTimeSetUp]
    public async Task TestSetup()
    {
        await BeforeEachTest();
    }

    [OneTimeTearDown]
    public async Task TestTeardown()
    {
        await AfterEachTest();
    }
}

[TestFixture]
public class PlaceBookOnHoldTestsBase : TestBase
{
    [Test]
    public async Task Succeeds()
    {
        var dbInitCommand = PlaceBookOnHoldInitDbCommandBuilder
            .InitAllTables()
            .WithRegularPatron(PlaceBookOnHoldTestData.RegularPatronId)
            .WithCirculatingBook(PlaceBookOnHoldTestData.CirculatingBookId)
            .Build();
        
        await SeedDatabase(dbInitCommand);
        
        await ReservationModule.ExecuteCommandAsync(new PlaceBookOnHoldCommand(
            PlaceBookOnHoldTestData.CirculatingBookId, 
            PlaceBookOnHoldTestData.RegularPatronId));

        var outboxNotification = await GetSingleOutboxMessage<BookPlacedOnHoldNotification>();
        
        Assert.That(outboxNotification,Is.Not.Null);
        Assert.That(outboxNotification.DomainEvent,Is.Not.Null);
        Assert.That(outboxNotification.DomainEvent.BookId, Is.EqualTo(new BookId(PlaceBookOnHoldTestData.CirculatingBookId)));
        Assert.That(outboxNotification.DomainEvent.PatronId, Is.EqualTo(new PatronId(PlaceBookOnHoldTestData.RegularPatronId)));
    }
}