namespace Dictionary.Data.Models;

public class Word
{
    public int Id { get; set; }
    public string Value { get; set; }
    public List<PossibleTranslation> PossibleTranslations { get; set; }
    public virtual List<Relation> RelatedWords { get; set; }
    public virtual List<Relation> RelatedFromWords { get; set; }
}