using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure;
using Library.Modules.Reservation.Infrastructure.Outbox;
using Newtonsoft.Json;
using Npgsql;

namespace Library.Modules.Reservation.IntegrationTests;

public abstract class TestBase
{
    protected static ReservationModule ReservationModule => new();
    protected static async Task SeedDatabase(string dbCommand)
    {
        await using var connection = new NpgsqlConnection(TestSettings.DbConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(dbCommand, connection);
        await command.ExecuteNonQueryAsync();
    }

    protected async Task<T> GetSingleOutboxMessage<T>()
        where T : class, IDomainNotification
    {
        await using var connection = new NpgsqlConnection(TestSettings.DbConnectionString);
        await connection.OpenAsync();
        var messages = await connection.QueryAsync<OutboxMessageDto>("SELECT * from reservations.outbox_messages");
        var messageList = messages.AsList();
        var type = Assembly.GetAssembly(typeof(T)).GetType(typeof(T).FullName);
        var message = messageList.SingleOrDefault();
        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }
}