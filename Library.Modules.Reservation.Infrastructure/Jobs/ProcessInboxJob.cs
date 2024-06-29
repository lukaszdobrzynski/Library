using Dapper;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Inbox;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public class ProcessInboxJob : IBackgroundJob
{
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    
    private const int BatchSize = 5;

    public ProcessInboxJob(IMediator mediator, IPsqlConnectionFactory connectionFactory, ILogger logger)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public async Task Run()
    {
        using (var connection = _connectionFactory.CreateNewConnection())
        {
            const string sql = "SELECT id, type, data " +
                               "FROM reservations.inbox_messages " +
                               "WHERE processed_at IS NULL " +
                               "ORDER BY occurred_on " +
                               "LIMIT @BatchSize;";
            
            var messages = await connection.QueryAsync<InboxMessageDto>(
                sql, new { BatchSize });
            var messageList = messages.ToList();

            if (messageList.Any() == false)
            {
                _logger.Information("No inbox messages to process.");
                return;
            }
            
            const string updateProcessedAtSql =
                "UPDATE reservations.inbox_messages " +
                "SET processed_at = @Date " +
                "WHERE id = @Id;";
            
            foreach (var message in messageList)
            {
                var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .Single(x => message.Type.Contains(x.GetName().Name));

                var messageType = messageAssembly.GetType(message.Type);
                var notification = JsonConvert.DeserializeObject(message.Data, messageType);
                
                await _mediator.Publish((INotification)notification);
                
                await connection.ExecuteAsync(updateProcessedAtSql, new
                    {
                        Date = DateTime.UtcNow,
                        message.Id,
                    }
                );
            }
        }
    }
}