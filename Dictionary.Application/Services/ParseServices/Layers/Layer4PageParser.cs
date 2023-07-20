using AngleSharp;
using Dictionary.Application.Models.Parsers;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Services.ParseServices.Layers;

//Рівень слова
public class Layer4PageParser : PageParser
{
    public Layer4PageParser(int layer, string baseUrl, ILogger logger) : base(layer, baseUrl, logger)
    {
    }

    public override async Task<IEnumerable<ParseResult>> ParseAsync(string url)
    {
        using var context = BrowsingContext.New(Config);
        using var document =
            await context.OpenAsync($"{BaseUrl}{url}");

        var elementEnWord = document.QuerySelector(".tw-bw.dhw.dpos-h_hw.di-title");
        if (elementEnWord is null)
        {
            Logger.LogError("Word not found! {BaseUrl}{Url}", BaseUrl, url);
            return new List<ParseResult> { new() { Source = url } };
        }

        var parseResult = new ParseResult
        {
            EnWord = elementEnWord.TextContent,
            LanguagePart = document.QuerySelector(".pos.dpos")?.TextContent
        };

        var divPosibleTanslations = document.QuerySelectorAll(".def-block.ddef_block");
        foreach (var div in divPosibleTanslations)
        {
            parseResult.PosibleTranslations.Add(new PosibleTranslation
            {
                Explanation = div.QuerySelector(".def.ddef_d.db")?.TextContent,
                Translation = div.QuerySelector(".trans.dtrans")?.TextContent,
                Example = div.QuerySelector(".eg.deg")?.TextContent
            });
        }

        var elementRelatedWords = document.QuerySelectorAll(".phrase");
        parseResult.RelatedWords = elementRelatedWords.Select(x => x.TextContent).ToList();

        return new List<ParseResult> { parseResult };
    }
}