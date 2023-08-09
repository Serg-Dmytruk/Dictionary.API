namespace Dictionary.Contacts.Responses;

public class WordResponse
{
    public int Id { get; set; }
    public string Value { get; set; }
    public List<PossibleTranslationResponse>? PossibleTranslations { get; set; }
    public virtual List<WordResponse>? RelatedWords { get; set; }
    public virtual List<WordResponse>? RelatedFromWords { get; set; }
}


public class PossibleTranslationResponse
{
    public string? Explanation { get; set; }
    public string? Translation { get; set; }
    public string? Example { get; set; }
}