using System.Data;

namespace Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;

internal interface IPsqlConnectionFactory
{
    IDbConnection GetOpenConnection();
    IDbConnection CreateNewConnection();
}