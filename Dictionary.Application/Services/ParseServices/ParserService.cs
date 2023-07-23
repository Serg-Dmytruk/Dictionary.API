using System.Diagnostics;
using Dictionary.Application.Models.Parsers;
using Dictionary.Application.Options;
using Dictionary.Application.Repositories;
using Dictionary.Application.Services.ParseServices.Factories;
using Dictionary.Data.Contexts;
using Dictionary.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dictionary.Application.Services.ParseServices
{
    public class ParserService : BackgroundService
    {
        private const int BaseLayer = 1;
        private const int WordLevel = 4;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<ParserOptions> _options;
        private readonly ILogger<ParserService> _parseLogger;

        public ParserService(IServiceProvider serviceProvider, IOptions<ParserOptions> options,
            ILogger<ParserService> parseLogger)
        {
            _serviceProvider = serviceProvider;
            _options = options;
            _parseLogger = parseLogger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _parseLogger.LogWarning("Start parse");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Посинаєм парсити
            var parser = ParserFactory.CreatePageParser(BaseLayer, _options.Value.BaseUrl, _parseLogger);
            var results = (await parser.ParseAsync("/dictionary/english-ukrainian")).ToList();

            //Вибераємо ті слова де парс навернувся через нагрузку
            var failedPages = results.Where(x => !string.IsNullOrEmpty(x.Source));
            var failedTask = (from failPage in failedPages
                let wordParser = ParserFactory.CreatePageParser(WordLevel, _options.Value.BaseUrl, _parseLogger)
                select wordParser.ParseAsync(failPage.Source!)).ToList();

            //Парсимо повторно і докідаємо результат до решти
            var secondTryResults = await Task.WhenAll(failedTask);
            foreach (var result in secondTryResults)
                results.AddRange(result);

            //Приводимо до моделі в бд
            var dictionary = results.Select(x => new Word
            {
                Value = x.EnWord,
                PossibleTranslations = x.PosibleTranslations.Select(p => new PossibleTranslation
                {
                    Translation = p.Interpretation,
                    Explanation = p.Explanation,
                    Example = p.Example
                }).ToList()
            }).ToList();

            await using var scope = _serviceProvider.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var transaction = await db.Database.BeginTransactionAsync(stoppingToken);
            var dictionaryRepository = scope.ServiceProvider.GetRequiredService<DictionaryRepository>();

            //вибераємо на добавлення в базу ті слова яких нема в базі
            var uniqueWords = await dictionaryRepository.GetUniqueRecords(dictionary);
            dictionary = dictionary.Where(x => uniqueWords.Contains(x.Value)).ToList();

            if (dictionary.Count > 0)
                await dictionaryRepository.AddRangeAsync(db, dictionary, transaction);

            /*ShowDebug(dictionary);*/

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            _parseLogger.LogWarning(
                "Elapsed milliseconds: {ElapsedMilliseconds} ms, number added words {DictionaryCount}",
                elapsedMilliseconds, dictionary.Count);
        }

        private static void ShowDebug(IReadOnlyCollection<Word> parseResults)
        {
            foreach (var result in parseResults.Where(x => !string.IsNullOrEmpty(x.Value)))
            {
                Console.WriteLine(result.Value);
                Console.WriteLine("+++++++++++++++++++");
            }

            foreach (var result in parseResults.Where(x => string.IsNullOrEmpty(x.Value)))
            {
                Console.WriteLine(result.Value);
                /*Console.WriteLine(result.LanguagePart);
                foreach (var tr in result.PosibleTranslations)
                {
                    Console.WriteLine(tr.Example);
                    Console.WriteLine(tr.Interpretation);
                    Console.WriteLine(tr.Explanation);
                    Console.WriteLine();
                }

                if (result?.RelatedWords != null)
                    foreach (var r in result.RelatedWords)
                    {
                        Console.WriteLine(r);
                    }*/

                Console.WriteLine("----------------------");
            }
        }
    }
}