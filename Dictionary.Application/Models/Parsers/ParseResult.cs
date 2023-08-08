namespace Dictionary.Application.Models.Parsers
{
    public class ParseResult
    {
        public string? Source { get; set; }
        public string EnWord { get; set; }
        public string? LanguagePart { get; set; }
        
        public List<Translation> PosibleTranslations = new();
        public IEnumerable<string>? RelatedWords { get; set; }
    }

    public class Translation
    {
        public string? Explanation { get; set; }
        public string? Interpretation { get; set; }
        public string? Example { get; set; }
    }
}