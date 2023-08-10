using Dictionary.Application.Models.Dictionaries;

namespace Dictionary.Application.Repositories.DictionaryRepositories;

public interface IDictionaryRepository
{
    Task<List<WordOut>> GetWordAsync(string value, CancellationToken cancellationToken);
}