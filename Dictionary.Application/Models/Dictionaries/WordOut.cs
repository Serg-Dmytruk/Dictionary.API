using Dictionary.Data.Models;

namespace Dictionary.Application.Models.Dictionaries;

public class WordOut
{
    public string Value { get; init; } = null!;
    public string? LanguagePart { get; init; }
    public IEnumerable<PossibleTranslation>? PossibleTranslations { get; init; }
    public virtual IEnumerable<string>? RelatedWords { get; init; }
    public virtual IEnumerable<string>? RelatedFromWords { get; init; }
}



