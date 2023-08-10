using Dictionary.Data.Models;

public class Word
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public string? LanguagePart { get; set; }
    public List<PossibleTranslation> PossibleTranslations { get; set; } = new();
    public virtual List<Relation> RelatedWords { get; set; } = new ();
    public virtual List<Relation> RelatedFromWords { get; set; } = new ();
}