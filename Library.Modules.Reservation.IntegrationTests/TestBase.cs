using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Library.BuildingBlocks.IntegrationTests;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Library.Modules.Reservation.Infrastructure.Outbox;
using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace Library.Modules.Reservation.IntegrationTests;

public abstract class TestBase
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithUsername(TestSettings.Username)
        .WithPassword(TestSettings.Password)
        .WithPortBinding(5432, true)
        .WithHostname(TestSettings.Hostname)
        .Build();

    private readonly Dictionary<string, Guid> _dbNames = new ();

    [OneTimeSetUp]
    public async Task SetUpOnceBeforeTests()
    {
        await _container.StartAsync();
    }

    [OneTimeTearDown]
    public async Task TearDownOnceAfterTests()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }
    
    [SetUp]
    public async Task BeforeEachTest()
    {
        var dbName = Guid.NewGuid();
        _dbNames.Add(TestContext.CurrentContext.Test.FullName, dbName);
        await ExecuteDbCommand($"CREATE DATABASE \"{dbName}\";", _container.GetConnectionString());
        var newDbConnectionString = TestSettings.DbConnectionString(_container.GetMappedPublicPort(5432), dbName);
        
        ReservationStartup.Init(
            newDbConnectionString, 
            new MockExecutionContextAccessor(), 
            new MockLogger(), 
            new MockEventBus());
    }
    
    protected static ReservationModule ReservationModule => new();
    protected async Task SeedDatabase(string dbCommand)
    {
        var dbName = _dbNames[TestContext.CurrentContext.Test.FullName];
        await ExecuteDbCommand(dbCommand, TestSettings.DbConnectionString(_container.GetMappedPublicPort(5432), dbName));
    }

    protected async Task<T> GetSingleOutboxMessage<T>()
        where T : class, IDomainNotification
    {
        var dbName = _dbNames[TestContext.CurrentContext.Test.FullName];
        var connString = TestSettings.DbConnectionString(_container.GetMappedPublicPort(5432), dbName);
        
        await using var connection = new NpgsqlConnection(connString);
        await connection.OpenAsync();
        var messages = await connection.QueryAsync<OutboxMessageDto>("SELECT * from reservations.outbox_messages;");
        var messageList = messages.AsList();
        var type = Assembly.GetAssembly(typeof(T)).GetType(typeof(T).FullName);
        var message = messageList.SingleOrDefault();
        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }

    private static async Task ExecuteDbCommand(string dbCommandText, string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(dbCommandText, connection);
        await command.ExecuteNonQueryAsync();
    }
}