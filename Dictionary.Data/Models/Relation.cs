namespace Dictionary.Data.Models;

public class Relation
{
    public int WordId { get; set; }
    public int RelatedWordId  { get; set; }
    public Word Word { get; set; } = null!;
    public Word RelatedWord { get; set; } = null!;
}