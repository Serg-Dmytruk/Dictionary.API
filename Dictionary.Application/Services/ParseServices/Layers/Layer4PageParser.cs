using AngleSharp;
using AngleSharp.Dom;
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
            await context.OpenAsync(
                $"{BaseUrl}{url}");
        var elementEnWords = document.QuerySelectorAll(".link.dlink");
        
        if (elementEnWords.Length == 0)
        {
            Logger.LogError("Word not found! {BaseUrl}{Url}", BaseUrl, url);
            return new List<ParseResult> { new() { Source = url } };
        }

        var result = new List<ParseResult>();
        var elementRelatedWords = document.QuerySelectorAll(".phrase").Select(x => x.TextContent);
        
        foreach (var htmlWordElement in elementEnWords)
        {
            var enWord = htmlWordElement.QuerySelector(".tw-bw.dhw.dpos-h_hw.di-title")?.TextContent;
            if (string.IsNullOrEmpty(enWord))
            {
                Logger.LogError("Word not found in div! {BaseUrl}{Url}", BaseUrl, url);
                result.Add(new ParseResult { Source = url });
                continue;
            }

            var parseResult = new ParseResult
            {
                EnWord = enWord,
                LanguagePart = htmlWordElement.QuerySelector(".pos.dpos")?.TextContent
            };

            var divPosibleTanslations = htmlWordElement.QuerySelectorAll(".def-block.ddef_block");
            parseResult.PosibleTranslations = divPosibleTanslations.Select(posibleTanslation => new Translation
            {
                Explanation = posibleTanslation.QuerySelector(".def.ddef_d.db")?.TextContent,
                Interpretation = posibleTanslation.QuerySelector(".trans.dtrans")?.TextContent,
                Examples = posibleTanslation.QuerySelectorAll(".eg.deg").Select(x => x.TextContent)
            });

            foreach (var divPosibleTanslation in divPosibleTanslations)
                divPosibleTanslation.RemoveFromParent();

            parseResult.RelatedWords = elementRelatedWords ?? Enumerable.Empty<string>();
            result.Add(parseResult);
            htmlWordElement.RemoveFromParent();
        }

        return result
            .GroupBy(r => new { r.LanguagePart, r.EnWord })
            .Select(group => new ParseResult
            {
                EnWord = group.Key.EnWord,
                LanguagePart = group.Key.LanguagePart,
                PosibleTranslations =
                    group.SelectMany(r => r.PosibleTranslations),
                RelatedWords = group.SelectMany(r => r.RelatedWords ?? Enumerable.Empty<string>())
            }).ToList();
    }
}