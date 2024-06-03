using Dapper;
using Library.BuildingBlocks.Application.Data;
using Library.Modules.Reservation.Infrastructure.Outbox;
using MediatR;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public class ProcessOutboxJob : IBackgroundJob
{
    private readonly IMediator _mediator;
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly ILogger _logger;

    private const int BatchSize = 10;
    
    public ProcessOutboxJob(
        IMediator mediator,
        IPsqlConnectionFactory connectionFactory, 
        ILogger logger)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public async Task Run()
    {
        using (var connection = _connectionFactory.CreateNewConnection())
        using (var transaction = connection.BeginTransaction())
        {
            const string sql = "SELECT id, type, data " +
                               "FROM reservations.outbox_messages " +
                               "WHERE processed_at IS NULL " +
                               "ORDER BY occurred_on " +
                               "LIMIT @BatchSize " +
                               "FOR UPDATE SKIP LOCKED;";
    
            var messages = await connection.QueryAsync<OutboxMessageDto>(
                sql, new { BatchSize }, transaction: transaction);
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
                await connection.ExecuteAsync(updateProcessedAtSql, new
                    {
                        Date = DateTime.UtcNow,
                        message.Id,
                        transaction
                    }
                );
            }
            
            transaction.Commit();
        }
    }
}