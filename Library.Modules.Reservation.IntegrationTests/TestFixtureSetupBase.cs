using System.Threading.Tasks;
using Library.BuildingBlocks.EventBus;
using Library.BuildingBlocks.IntegrationTests;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Testcontainers.PostgreSql;

namespace Library.Modules.Reservation.IntegrationTests;

public class TestFixtureSetupBase
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithUsername(TestSettings.Username)
        .WithPassword(TestSettings.Password)
        .WithDatabase(TestSettings.DbName)
        .WithPortBinding(TestSettings.HostPort, 5432)
        .WithHostname(TestSettings.Hostname)
        .Build();
    
    protected async Task BeforeEachTest()
    {
        ReservationStartup.Init(
            TestSettings.DbConnectionString, 
            new MockExecutionContextAccessor(), 
            new MockLogger(), 
            new InMemoryEventBus());
        
        await _postgres.StartAsync();
    }
    
    protected async Task AfterEachTest()
    {
        await _postgres.StopAsync();
        await _postgres.DisposeAsync();
    }
}