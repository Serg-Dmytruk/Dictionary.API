using System.Data;
using System.Data.Common;
using Dictionary.Data.Models;

namespace Dictionary.Application.Repositories.ParserRepositories;

public interface IParserRepository : IEfCoreDefaultRepository
{
    Task AddRelations(string json, DbConnection connection, IDbTransaction? transaction);
    Task<List<string>> GetUniqueRecords(IEnumerable<Word> words, DbConnection connection, IDbTransaction? transaction);
}