using System.Diagnostics;
using Dictionary.Application.Models.Parsers;
using Dictionary.Application.Options;
using Dictionary.Application.Services.ParseServices.Factories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dictionary.Application.Services.ParseServices
{
    public class ParserService : BackgroundService
    {
        private const int BaseLayer = 1;
        private const int WordLevel = 4;
        private readonly IOptions<ParserOptions> _options;
        private readonly ILogger<ParserService> _parseLogger;

        public ParserService(IOptions<ParserOptions> options, ILogger<ParserService> parseLogger)
        {
            _options = options;
            _parseLogger = parseLogger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var parser = ParserFactory.CreatePageParser(BaseLayer, _options.Value.BaseUrl, _parseLogger);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var results = (await parser.ParseAsync("/dictionary/english-ukrainian")).ToList();
            var failedPages = results.Where(x => !string.IsNullOrEmpty(x.Source));

            //TODO
            var failedTask = (from failPage in failedPages
                let wordParser = ParserFactory.CreatePageParser(WordLevel, _options.Value.BaseUrl, _parseLogger)
                select wordParser.ParseAsync(failPage.Source!)).ToList();

            var secondTryResults = await Task.WhenAll(failedTask);
            
            var parseResults = new List<ParseResult>();
            foreach (var result in secondTryResults)
            {
                parseResults.AddRange(result);
            }
            
            results.AddRange(parseResults);
            foreach (var result in results)
            {
                Console.WriteLine(result.EnWord);
                Console.WriteLine(result.LanguagePart);
                foreach (var tr in result.PosibleTranslations)
                {
                    Console.WriteLine(tr.Example);
                    Console.WriteLine(tr.Translation);
                    Console.WriteLine(tr.Explanation);
                    Console.WriteLine();
                }

                if (result?.RelatedWords != null)
                    foreach (var r in result.RelatedWords)
                    {
                        Console.WriteLine(r);
                    }

                Console.WriteLine("----------------------");
            }

            Console.WriteLine(results.Count());
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Час виконання: {elapsedMilliseconds} мс");
        }
    }
}