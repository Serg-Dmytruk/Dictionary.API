namespace Dictionary.Application.Models.Parsers
{
    public class ParseResult
    {
        public string? Source { get; init; }
        public string EnWord { get; init; } = null!;
        public string? LanguagePart { get; init; }
        public IEnumerable<Translation> PosibleTranslations { get; set; } = null!;
        public IEnumerable<string>? RelatedWords { get; set; }
    }

    public class Translation
    {
        public string? Explanation { get; init; }
        public string? Interpretation { get; init; }
        public IEnumerable<string> Examples { get; init; } = null!;
    }
}