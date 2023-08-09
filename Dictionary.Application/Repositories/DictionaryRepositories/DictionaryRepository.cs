using Dictionary.Application.Models.Dictionaries;
using Dictionary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Application.Repositories.DictionaryRepositories;

public class DictionaryRepository : IDictionaryRepository
{
    private readonly ApplicationDbContext _db;

    public DictionaryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Models.Dictionaries.WordOut?> GetWordAsync(string value, CancellationToken cancellationToken)
    {
        var word = await _db.Words
            .Include(w => w.PossibleTranslations)
            .Include(w => w.RelatedWords).ThenInclude(x => x.RelatedWord)
            .Include(w => w.RelatedFromWords).ThenInclude(x => x.Word)
            .AsNoTracking()
            .SingleOrDefaultAsync(w => w.Value.ToLower() == value.ToLower(), cancellationToken);
        
        return word is null ? null : new Models.Dictionaries.WordOut
        {
            Value = word.Value,
            PossibleTranslations = word.PossibleTranslations,
            RelatedWords = word.RelatedWords?.Select(x => x.RelatedWord.Value),
            RelatedFromWords = word.RelatedFromWords?.Select(x => x.Word.Value)
        };
    }
}