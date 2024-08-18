using Dapper;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.DailySheet;
using MediatR;
using Serilog;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public class ProcessDailySheetJob : IBackgroundJob
{
    private readonly IPsqlConnectionFactory _connectionFactory;
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public ProcessDailySheetJob(IMediator mediator, IPsqlConnectionFactory connectionFactory, ILogger logger)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public async Task Run()
    {
        using (var connection = _connectionFactory.CreateNewConnection())
        {
            const string sql = "SELECT id, bookId, patronId, libraryBranchId, till " +
                               "FROM reservations.holds " +
                               "WHERE status = 'Granted' OR status = 'ReadyToPick' AND till < now()::date;";
                               
            var holds = await connection.QueryAsync<HoldDto>(sql);
            var holdList = holds.ToList();

            if (holdList.Any() == false)
            {
                _logger.Information("No holds to process.");
                return;
            }
            
            const string updateHoldStatusSql =
                "UPDATE reservations.holds " +
                "SET status = 'Cancelled', isActive = false " +
                "WHERE id = @Id;";

            foreach (var holdItem in holdList)
            {
                //TODO: send notification to the Catalogue context to make a book Available
                
                await connection.ExecuteAsync(updateHoldStatusSql, new
                {
                    holdItem.Id
                });
            }
        }
    }
}