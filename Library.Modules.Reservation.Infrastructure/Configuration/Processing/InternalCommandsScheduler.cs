using Dapper;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Newtonsoft.Json;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class InternalCommandsScheduler : IInternalCommandsScheduler
{
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly IInternalCommandsRegistry _internalCommandsRegistry;
    
    public InternalCommandsScheduler(IPsqlConnectionFactory connectionFactory, IInternalCommandsRegistry internalCommandsRegistry)
    {
        _connectionFactory = connectionFactory;
        _internalCommandsRegistry = internalCommandsRegistry;
    }
    
    public async Task Submit(ICommand command)
    {
        using (var connection  = _connectionFactory.CreateNewConnection())
        {
            const string sqlInsert = "INSERT INTO reservations.internal_commands (id, created_at, type, data) VALUES " +
                                     "(@Id, @CreatedAt, @Type, @Data::json)";

            var data = JsonConvert.SerializeObject(command);

            await connection.ExecuteScalarAsync(sqlInsert, new
            {
                command.Id,
                CreatedAt = DateTime.UtcNow,
                Type = _internalCommandsRegistry.GetName(command.GetType()),
                Data = data
            });
        }    
    }
}