using AngleSharp;
using Dictionary.Application.Models.Parsers;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Services.ParseServices.Layers;

//рівень алфавіту
public class Layer1PageParser : PageParser
{
    public Layer1PageParser(int layer, string baseUrl, ILogger logger) : base(layer, baseUrl, logger)
    {
    }

    public override async Task<IEnumerable<ParseResult>> ParseAsync(string url)
    {
        try
        {
            using var context = BrowsingContext.New(Config);
            using var document = await context.OpenAsync($"{BaseUrl}{url}");

            var div = document.QuerySelector(".hax.fs19.tb");

            if (div is null)
                return Enumerable.Empty<ParseResult>();
            var links = div.QuerySelectorAll("a");
            var hrefs = links.Select(a =>
                a.GetAttribute("href"));

            var childParseTasks =  CteateChildParsers(hrefs);
            var childParseResults = await Task.WhenAll(childParseTasks);

            // Обробка результатів з вкладених парсерів дочірнього шару та побудова спільного результату
            var parseResult = new List<ParseResult>();
            foreach (var result in childParseResults)
            {
                parseResult.AddRange(result);
            }

            return parseResult;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "error ocured Layer1PageParser");
            return Enumerable.Empty<ParseResult>();
        }
    }
}