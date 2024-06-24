using Dapper;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.Processing;
using MediatR;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public class ProcessInternalCommandJob : IBackgroundJob
{
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    private const int BatchSize = 1;

    public ProcessInternalCommandJob(IPsqlConnectionFactory connectionFactory, IMediator mediator, ILogger logger)
    {
        _connectionFactory = connectionFactory;
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Run()
    {
        using (var connection = _connectionFactory.CreateNewConnection())
        using (var transaction = connection.BeginTransaction())
        {
            const string sql = "SELECT id, type, data " +
                               "FROM reservations.internal_commands " +
                               "WHERE processed_at IS NULL " +
                               "ORDER BY created_at " +
                               "LIMIT @BatchSize " +
                               "FOR UPDATE SKIP LOCKED;";
            
            var commands = await connection.QueryAsync<InternalCommandDto>(
                sql, new { BatchSize }, transaction: transaction);
            var commandList = commands.ToList();

            if (commandList.Any() == false)
            {
                _logger.Information("No internal commands to process.");
                return;
            }
            
            foreach (var internalCommand in commandList)
            {
                //var type = _internalCommandsMapper.GetType(internalCommand.Type);
                //dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

                try
                {
                    //await CommandsExecutor.Execute(commandToProcess);
                    
                    const string updateProcessedAtSql =
                        "UPDATE reservations.internal_commands " +
                        "SET processed_at = @ProcessedAt " +
                        "WHERE id = @Id;";
                
                    await connection.ExecuteScalarAsync(
                        updateProcessedAtSql,
                        new
                        {
                            internalCommand.Id,
                            ProcessAt = DateTime.UtcNow
                        });
                }
                catch (Exception e)
                {
                    // marked commands failed
                }
            }
            
            transaction.Commit();
        }    
        
    }
}