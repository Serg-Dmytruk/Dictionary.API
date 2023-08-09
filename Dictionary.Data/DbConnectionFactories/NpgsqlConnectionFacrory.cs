using System.Data;
using Npgsql;

namespace Dictionary.Data.DbConnectionFactories;

public class NpgsqlConnectionFacrory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFacrory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
} 