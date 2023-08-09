using Dictionary.Data.Models;

namespace Dictionary.Application.Models.Dictionaries;

public class WordOut
{
    public string Value { get; set; } = null!;
    public IEnumerable<PossibleTranslation>? PossibleTranslations { get; set; }
    public virtual IEnumerable<string>? RelatedWords { get; set; }
    public virtual IEnumerable<string>? RelatedFromWords { get; set; }
}



