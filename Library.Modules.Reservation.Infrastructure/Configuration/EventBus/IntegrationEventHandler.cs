﻿using System.Data;
using Autofac;
using Dapper;
using Library.BuildingBlocks.EventBus;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Inbox;
using Newtonsoft.Json;

namespace Library.Modules.Reservation.Infrastructure.Configuration.EventBus;

public class IntegrationEventHandler<T> : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public async Task Handle(T integrationEvent)
    {
        var scope = ReservationCompositionRoot.BeginLifetimeScope();
        var connectionFactory = scope.Resolve<IPsqlConnectionFactory>();

        using (var connection = connectionFactory.CreateNewConnection())
        using (var transaction = connection.BeginTransaction())
        {
            if (await IsMessageAlreadyStored(connection, transaction, integrationEvent.Id))
                return;
            
            var type = integrationEvent.GetType().FullName;
            var data = JsonConvert.SerializeObject(integrationEvent);

            const string insertSql = "INSERT INTO reservations.inbox_messages (id, occurred_on, data, type) " +
                                     "VALUES (@Id, @OccurredOn, @Data::json, @Type);";

            await connection.ExecuteScalarAsync(insertSql,
                new { 
                    integrationEvent.Id, 
                    OccurredOn = DateTime.UtcNow,
                    data,
                    type });
            
            transaction.Commit();
        }
    }
    
    private static async Task<bool> IsMessageAlreadyStored(IDbConnection connection, IDbTransaction transaction, Guid notificationId)
    {
        const string sql = "SELECT id FROM reservations.inbox_messages WHERE id = @Id;";
        var messages = await connection.QueryAsync<InboxMessage>(sql, new { Id = notificationId, transaction });
        var messagesList = messages.ToList();

        return messagesList.Any();
    }
}