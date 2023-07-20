using Dictionary.Data.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;

namespace Dictionary.Application.Repositories;

public class DictionaryRepository
{
    private readonly string? _connectionString;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<DictionaryRepository> _logger;

    public DictionaryRepository(IConfiguration configuration, ApplicationDbContext db, ILogger<DictionaryRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _db = db;
        _logger = logger;
    }

    //TODO
    public async Task<List<TReturn>> GetUniqueRecords<T, TReturn>(IEnumerable<T> logs, string table)
        where T : class
    {
        await using var connection = new SqlConnection (_connectionString);
        await connection.OpenAsync();
        var query = "";
        
        return (await connection.QueryAsync<TReturn>(query)).ToList();
    }
}