using Library.BuildingBlocks.IntegrationTests;
using Library.Modules.Catalogue.Infrastructure.Configuration;
using NUnit.Framework;

namespace Library.Modules.Catalogue.IntegrationTests;

public class TestBase : RavenTestBase
{
    [SetUp]
    protected override void BeforeEachTest()
    {
        base.BeforeEachTest();
        CatalogueStartup.Init(
            DocumentStoreHolder,
            new MockExecutionContextAccessor(),
            new MockLogger(),
            new MockEventBus());
    }
}