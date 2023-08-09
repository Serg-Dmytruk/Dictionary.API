using Dictionary.Data.DbConnectionFactories;
using Dictionary.Data.OutModels;

namespace Dictionary.Application.Repositories.DictionaryRepositories;

public class DictionaryReposytory : IDictionaryReposytory
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public DictionaryReposytory(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<WordOut> GetWordAsync()
    {
       using var connection = await _dbConnectionFactory.CreateConnectionAsync();
       return null;

    }
}