using System.Data;

namespace Library.BuildingBlocks.Application.Data;

public interface IPsqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}