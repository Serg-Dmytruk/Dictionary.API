using Dictionary.Application.Models.Dictionaries;

namespace Dictionary.Application.Repositories.DictionaryRepositories;

public interface IDictionaryRepository
{
    Task<Models.Dictionaries.WordOut?> GetWordAsync(string value, CancellationToken cancellationToken);
}