using System.Data;
using System.Data.Common;
using System.Text.Json;
using Dapper;
using Dictionary.Data.Models;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Repositories.ParserRepositories;

public class ParserRepository : EfCoreDefaultRepository, IParserRepository
{

    public ParserRepository(ILogger<ParserRepository> logger) : base(logger)
    {
    }

    public async Task AddRelations(string json, DbConnection connection, IDbTransaction? transaction)
    {
        var query = $@"WITH words_relations AS (SELECT
                    json_data->>'EnWord' AS word,
                    related.RelatedWords AS related_word
                FROM 
                    json_array_elements('{json}') AS json_data,
	                json_array_elements_text(json_data->'RelatedWords') AS related(RelatedWords))

                INSERT INTO relations (word_id, related_word_id)
                SELECT DISTINCT w1.id AS word_id, w2.id AS related_word_id FROM words_relations wr
                LEFT JOIN words w1 ON wr.word = w1.value
                LEFT JOIN words w2 ON wr.related_word = w2.value
                WHERE w2 IS NOT NULL";

        await connection.ExecuteAsync(query, transaction:transaction);
    }

    public async Task<List<string>> GetUniqueRecords(IEnumerable<Word> words, DbConnection connection, IDbTransaction? transaction)
    {
        var json = JsonSerializer.Serialize(words);
        var query = $@"SELECT *
        FROM JSONB_TO_RECORDSET('{json}') AS t(""Value"" text)
        WHERE NOT EXISTS (
                SELECT 1 FROM words AS w 
                    WHERE t.""Value"" = w.value)";

        return (await connection.QueryAsync<string>(query, transaction:transaction)).ToList();
    }
}