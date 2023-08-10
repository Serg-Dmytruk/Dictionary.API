namespace Dictionary.Data.Models;

public class Example
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public int PossibleTranslationId { get; set; }
    public PossibleTranslation PossibleTranslation { get; set; } = null!;
}