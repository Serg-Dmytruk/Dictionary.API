namespace Dictionary.Data.Models;

public class PossibleTranslation
{
    public int Id { get; set; }
    public int WordId { get; set; }
    public string? Explanation { get; set; }
    public string? Translation { get; set; }
    public string? Example { get; set; }
    public Word Word { get; set; } = null!;
}