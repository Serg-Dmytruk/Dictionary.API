using Dictionary.Application.Models.Dictionaries;
using Dictionary.Application.Repositories.DictionaryRepositories;

namespace Dictionary.Application.Services.DictionaryServices;

public class DictionaryService : IDictionaryService
{
    private readonly IDictionaryRepository _dictionaryRepository;
    
    public DictionaryService(IDictionaryRepository dictionaryRepository)
    {
        _dictionaryRepository = dictionaryRepository;
    }
    
    public async Task<Models.Dictionaries.WordOut?> GetWordAsync(string request, CancellationToken cancellationToken)
    {
        return await  _dictionaryRepository.GetWordAsync(request, cancellationToken);
    }
}