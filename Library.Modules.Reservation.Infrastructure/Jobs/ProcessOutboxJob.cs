using Dapper;
using Library.BuildingBlocks.Application.Data;
using Library.Modules.Reservation.Infrastructure.Outbox;
using MediatR;

namespace Library.Modules.Reservation.Infrastructure.Jobs;

public class ProcessOutboxJob : IBackgroundJob
{
    private readonly IMediator _mediator;
    private readonly IPsqlConnectionFactory _connectionFactory;
    
    public ProcessOutboxJob(IMediator mediator, IPsqlConnectionFactory connectionFactory)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
    }
    
    public async Task Run()
    {
        var connection = _connectionFactory.GetOpenConnection();
        
        var sql = "SELECT * FROM reservations.outbox_messages;";
        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
        var messageList = messages.AsList();
    }
}