﻿using Dapper;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Application.Holds.CancelHold;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.DailySheet;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

internal class ProcessDailySheetJob : IBackgroundJob
{
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private readonly IInternalCommandsScheduler _internalCommandsScheduler;

    public ProcessDailySheetJob(IInternalCommandsScheduler internalCommandsScheduler, IPsqlConnectionFactory connectionFactory, ILogger logger)
    {
        _internalCommandsScheduler = internalCommandsScheduler;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public async Task Run()
    {
        using (var connection = _connectionFactory.CreateNewConnection())
        {
            const string sql = "SELECT id, book_id BookId, patron_id PatronId, library_branch_id LibraryBranchId, till " +
                               "FROM reservations.holds " +
                               "WHERE (status = 'Granted' OR status = 'ReadyToPick') AND till < now()::date;";
                               
            var holds = await connection.QueryAsync<HoldDto>(sql);
            var holdDtos = holds.ToList();

            if (holdDtos.Any() == false)
            {
                _logger.Information("No expired holds to process.");
                return;
            }
            
            foreach (var dto in holdDtos)
            {
                await _internalCommandsScheduler.Submit(new CancelExpiredHoldCommand
                {
                    Id = Guid.NewGuid(),
                    HoldId = dto.Id,
                    BookId = dto.BookId
                });
            }
        }
    }
}