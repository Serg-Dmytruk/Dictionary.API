using System.Data;
using Npgsql;

namespace Dictionary.Data.DbConnectionFactories;

public class NpsqlConnectionFacroty : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpsqlConnectionFacroty(string connectionString)
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