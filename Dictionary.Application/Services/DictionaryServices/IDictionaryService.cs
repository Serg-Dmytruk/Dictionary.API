using Dictionary.Application.Models.Dictionaries;

namespace Dictionary.Application.Services.DictionaryServices;

public interface IDictionaryService
{
    Task<Models.Dictionaries.WordOut?> GetWordAsync(string request, CancellationToken cancellationToken);
}