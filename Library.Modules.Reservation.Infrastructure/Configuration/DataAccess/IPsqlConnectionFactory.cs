using System.Data;

namespace Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;

public interface IPsqlConnectionFactory
{
    IDbConnection GetOpenConnection();
    IDbConnection CreateNewConnection();
}