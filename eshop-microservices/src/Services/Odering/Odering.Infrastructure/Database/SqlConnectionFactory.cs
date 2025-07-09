using Microsoft.Data.SqlClient;
using Npgsql;
using Ordering.Application.Configuration.Data;
using System.Data;

namespace Odering.Infrastructure.Database;

public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            // open postgres connection
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }

        return _connection;
    }

    public void Dispose()
    {
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Dispose();
        }
    }
}