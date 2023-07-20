using AngleSharp;
using Dictionary.Application.Models.Parsers;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Services.ParseServices.Layers;

//рівень розділів
public class Layer2PageParser : PageParser
{
    public Layer2PageParser(int layer, string baseUrl, ILogger logger) : base(layer, baseUrl, logger)
    {
    }

    public override async Task<IEnumerable<ParseResult>> ParseAsync(string url)
    {
        try
        {
            using var context = BrowsingContext.New(Config);
            using var document = await context.OpenAsync($"{BaseUrl}/{url}");

            var div = document.QuerySelector(".hdf.ff-50.lmt-15.i-browse");

            if (div is null)
                return Enumerable.Empty<ParseResult>();
           
            var links = div.QuerySelectorAll("a");
            var hrefs = links.Select(a =>
                a.GetAttribute("href"));

    
            var childParseTasks = CteateChildParsers(hrefs);
            var childParseResults = await Task.WhenAll(childParseTasks);

            var parseResult = new List<ParseResult>();
            foreach (var result in childParseResults)
            {
                parseResult.AddRange(result);
            }

            return parseResult;
        }
        catch (Exception e)
        {
            Logger.LogWarning(e, "error ocured {Url}", url);
            return Enumerable.Empty<ParseResult>();
        }
    }
}