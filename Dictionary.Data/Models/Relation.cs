namespace Dictionary.Data.Models;

public class Relation
{
    public int WordId { get; set; }
    public int RelatedWordId  { get; set; }
    public Word Word { get; set; }
    public Word RelatedWord  { get; set; }
}