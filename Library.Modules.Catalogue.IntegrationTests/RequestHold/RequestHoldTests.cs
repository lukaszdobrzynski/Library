using System.Threading.Tasks;
using NUnit.Framework;

namespace Library.Modules.Catalogue.IntegrationTests.RequestHold;

public class RequestHoldTests : TestBase
{
    [Test]
    public async Task Succeeds()
    {
        using (var session = OpenAsyncSession())
        {
            await session.StoreAsync(new TestData { Name = "Test Data 1"});
            await session.SaveChangesAsync();
        }
    }
}

public class TestData
{
    public string Name { get; set; }
}