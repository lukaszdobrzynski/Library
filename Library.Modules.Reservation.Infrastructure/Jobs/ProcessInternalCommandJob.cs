﻿using System.Data;
using Dapper;
using Library.BuildingBlocks.Infrastructure;
using Library.Modules.Reservation.Infrastructure.Configuration;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.Processing;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public class ProcessInternalCommandJob : IBackgroundJob
{
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private readonly IInternalCommandsRegistry _internalCommandsRegistry;
    private readonly AsyncRetryPolicy _retryPolicy;

    private const int BatchSize = 1;
    private const int RetryCount = 3;

    public ProcessInternalCommandJob(IPsqlConnectionFactory connectionFactory, 
        IRetryPolicyFactory retryPolicyFactory, 
        IInternalCommandsRegistry internalCommandsRegistry, 
        ILogger logger)
    {
        _connectionFactory = connectionFactory;
        _retryPolicy = retryPolicyFactory.RetryWithSleepDuration(RetryCount, TimeSpan.FromSeconds(1));
        _internalCommandsRegistry = internalCommandsRegistry;
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
                var type = _internalCommandsRegistry.GetType(internalCommand.Type);
                dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

                var result =
                    await _retryPolicy.ExecuteAndCaptureAsync(() => CommandsExecutor.Execute(commandToProcess));

                if (result.Outcome == OutcomeType.Failure)
                {
                    await SetCommandError(connection, internalCommand.Id, result.FinalException.ToString());
                }

                await SetCommandProcessed(connection, internalCommand.Id);
                
                transaction.Commit();
            }
        }    
    }

    private async Task SetCommandProcessed(IDbConnection connection, Guid commandId)
    {
        const string updateProcessedAtSql =
            "UPDATE reservations.internal_commands " +
            "SET processed_at = @ProcessedAt " +
            "WHERE id = @Id;";
        
        await connection.ExecuteScalarAsync(
            updateProcessedAtSql,
            new
            {
                Id = commandId,
                ProcessAt = DateTime.UtcNow
            });
    }

    private async Task SetCommandError(IDbConnection connection, Guid commandId, string error)
    {
        const string updateOnErrorSql = "UPDATE reservations.internal_commands " +
                                        "SET processed_at = @Date, " +
                                        "processing_error = @Error " +
                                        "WHERE id = @Id";

        await connection.ExecuteScalarAsync(
            updateOnErrorSql,
            new
            {
                Date = DateTime.UtcNow,
                Error = error,
                Id = commandId
            });
    }
}