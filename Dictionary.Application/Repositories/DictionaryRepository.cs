using Dictionary.Data.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Text.Json;
using Dapper;
using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace Dictionary.Application.Repositories;

public class DictionaryRepository : DefaultRepository
{
    private readonly string? _connectionString;
    

    public DictionaryRepository(IConfiguration configuration, ILogger<DictionaryRepository> logger) : base(logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task AddRelations(DbContext context, IDbContextTransaction transaction)
    {
        
    }
    
    public async Task<List<string>> GetUniqueRecords(IEnumerable<Word> words)
        
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var json = JsonSerializer.Serialize(words);
        var query = $@"SELECT *
        FROM JSONB_TO_RECORDSET('{json}') AS t(""Value"" text)
        WHERE NOT EXISTS (
                SELECT 1 FROM words AS w 
                    WHERE t.""Value"" = w.value)";
        
        return (await connection.QueryAsync<string>(query)).ToList();
    }
}