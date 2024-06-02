using System.Data;
using Library.BuildingBlocks.Application.Data;
using Npgsql;

namespace Library.BuildingBlocks.Infrastructure;

public class PsqlConnectionFactory : IPsqlConnectionFactory, IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;
    
    public PsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IDbConnection GetOpenConnection()
    {
        if (_connection is { State: ConnectionState.Open })
        {
            return _connection;
        }
            
        _connection = new NpgsqlConnection(_connectionString);
        _connection.Open();
        return _connection;
    }
    
    public void Dispose()
    {
        if (_connection is { State: ConnectionState.Open })
        {
            _connection.Dispose();    
        }
    }
}