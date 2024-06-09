using System.Data;
using Npgsql;

namespace Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;

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
    
    public IDbConnection CreateNewConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
    
    public void Dispose()
    {
        if (_connection is { State: ConnectionState.Open })
        {
            _connection.Dispose();    
        }
    }
}