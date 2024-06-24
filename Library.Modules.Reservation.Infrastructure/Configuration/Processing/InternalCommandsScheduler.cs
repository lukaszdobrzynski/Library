using Dapper;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Newtonsoft.Json;

namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class InternalCommandsScheduler : IInternalCommandsScheduler
{
    private readonly IPsqlConnectionFactory _connectionFactory;
    
    public InternalCommandsScheduler(IPsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task Submit(ICommand command)
    {
        using (var connection  = _connectionFactory.CreateNewConnection())
        using (var transaction = connection.BeginTransaction())
        {
            const string sqlInsert = "INSERT INTO reservations.internal_commands (id, created_at, type, data) VALUES " +
                                     "(@Id, @CreatedAt, @Type, @Data::json)";

            var data = JsonConvert.SerializeObject(command);

            await connection.ExecuteScalarAsync(sqlInsert, new
            {
                command.Id,
                CreatedAt = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = data
            });
            
            transaction.Commit();
        }    
    }
}