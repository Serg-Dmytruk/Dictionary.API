namespace Dictionary.Data.OutModels;

public class WordOut
{
    public int Id { get; set; }
    public string Value { get; set; }
    public List<PossibleTranslationOut>? PossibleTranslations { get; set; }
    public virtual List<WordOut>? RelatedWords { get; set; }
    public virtual List<WordOut>? RelatedFromWords { get; set; }
}

public class PossibleTranslationOut
{
    public string? Explanation { get; set; }
    public string? Translation { get; set; }
    public string? Example { get; set; }
}


