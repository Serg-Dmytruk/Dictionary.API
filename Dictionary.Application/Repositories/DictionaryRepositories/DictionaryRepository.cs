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

    public async Task<List<WordOut>> GetWordAsync(string value, CancellationToken cancellationToken)
    {
        var words = await _db.Words
            .Include(w => w.PossibleTranslations).ThenInclude(x => x.Examples)
            .Include(w => w.RelatedWords).ThenInclude(x => x.RelatedWord)
            .Include(w => w.RelatedFromWords).ThenInclude(x => x.Word)
            .AsNoTracking()
            .Where(w => w.Value.ToLower() == value.ToLower()).ToListAsync(cancellationToken);

        return words.Select(x => new WordOut
        {
            Value = x.Value,
            LanguagePart = x.LanguagePart,
            PossibleTranslations = x.PossibleTranslations,
            RelatedWords = x.RelatedWords.Select(c => c.RelatedWord.Value),
            RelatedFromWords = x.RelatedFromWords.Select(p => p.Word.Value)
        }).ToList();
    }
}