using Dapper;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Outbox;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public class ProcessOutboxJob : IBackgroundJob
{
    private readonly IMediator _mediator;
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private readonly IDomainNotificationsRegistry _notificationsRegistry;

    private const int BatchSize = 5;
    
    public ProcessOutboxJob(
        IMediator mediator,
        IPsqlConnectionFactory connectionFactory, 
        ILogger logger,
        IDomainNotificationsRegistry notificationsRegistry)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
        _logger = logger;
        _notificationsRegistry = notificationsRegistry;
    }
    
    public async Task Run()
    {
        using (var connection = _connectionFactory.CreateNewConnection())
        {
            const string sql = "SELECT id, type, data " +
                               "FROM reservations.outbox_messages " +
                               "WHERE processed_at IS NULL " +
                               "ORDER BY occurred_on " +
                               "LIMIT @BatchSize " +
                               "FOR UPDATE SKIP LOCKED;";
    
            var messages = await connection.QueryAsync<OutboxMessageDto>(
                sql, new { BatchSize });
            var messageList = messages.ToList();

            if (messageList.Any() == false)
            {
                _logger.Information("No outbox messages to process.");
                return;
            } 

            const string updateProcessedAtSql =
                "UPDATE reservations.outbox_messages " +
                "SET processed_at = @Date " +
                "WHERE id = @Id;";

            foreach (var message in messageList)
            {
                var type = _notificationsRegistry.GetType(message.Type);
                var notification = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;
                
                await _mediator.Publish(notification);
                
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