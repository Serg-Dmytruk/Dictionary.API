namespace Dictionary.Contacts.Responses;

public class WordResponse
{
    public string Value { get; set; } = null!;
    public List<PossibleTranslationResponse>? PossibleTranslations { get; set; }
    public virtual List<string>? RelatedWords { get; set; }
    public virtual List<string>? RelatedFromWords { get; set; }
}

public class PossibleTranslationResponse
{
    public string? Explanation { get; set; }
    public string? Translation { get; set; }
    public string? Example { get; set; }
}