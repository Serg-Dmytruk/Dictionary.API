using AngleSharp;
using Dictionary.Application.Models.Parsers;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Services.ParseServices.Layers;

//рівень слова в розділі
public class Layer3PageParser : PageParser
{
    private static readonly SemaphoreSlim Semaphore = new(1);

    public Layer3PageParser(int layer, string baseUrl, ILogger logger) : base(layer, baseUrl, logger)
    {
    }

    public override async Task<IEnumerable<ParseResult>> ParseAsync(string url)
    {
        try
        {
            await Semaphore.WaitAsync();
            using var context = BrowsingContext.New(Config);
            using var document = await context.OpenAsync(url);
            var div = document.QuerySelector(".hdf.ff-50.lmt-15.i-browse");

            if (div is null)
            {
                Semaphore.Release();
                return Enumerable.Empty<ParseResult>();
            }

            var hrefs = div.QuerySelectorAll("a").Select(a =>
                a.GetAttribute("href"));

            var childParseTasks = CteateChildParsers(hrefs);
            var childParseResults = await Task.WhenAll(childParseTasks);
            var parseResult = childParseResults.SelectMany(result => result);

            Semaphore.Release();
            return parseResult;
        }
        catch (Exception e)
        {
            Semaphore.Release();
            Logger.LogWarning(e, "error ocured Layer3PageParser {Url}", url);
            return Enumerable.Empty<ParseResult>();
        }
    }
}