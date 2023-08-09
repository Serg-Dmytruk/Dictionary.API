using System.Data;

namespace Dictionary.Data.DbConnectionFactories;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}