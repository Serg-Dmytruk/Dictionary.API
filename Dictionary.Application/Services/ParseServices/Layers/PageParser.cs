using AngleSharp;
using Dictionary.Application.Models.Parsers;
using Dictionary.Application.Services.ParseServices.Factories;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Services.ParseServices.Layers;

public abstract class PageParser
{
    private readonly int _layer;
    protected readonly string BaseUrl;
    protected readonly ILogger Logger;
    protected readonly IConfiguration Config = Configuration.Default.WithDefaultLoader();

    protected PageParser(int layer, string baseUrl, ILogger logger)
    {
        _layer = layer;
        BaseUrl = baseUrl;
        Logger = logger;
    }

    public abstract Task<IEnumerable<ParseResult>> ParseAsync(string url);
    
    protected IEnumerable<Task<IEnumerable<ParseResult>>> CteateChildParsers(IEnumerable<string?> hrefs)
    {
        return (from href in hrefs.Skip(1).Take(1)
            let childParser = ParserFactory.CreatePageParser(_layer + 1, BaseUrl, Logger)
            where !string.IsNullOrEmpty(href)
            select childParser.ParseAsync(href));
    }
}