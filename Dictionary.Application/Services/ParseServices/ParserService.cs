using System.Diagnostics;
using System.Text.Json;
using Dictionary.Application.Options;
using Dictionary.Application.Repositories;
using Dictionary.Application.Repositories.ParserRepositories;
using Dictionary.Application.Services.ParseServices.Factories;
using Dictionary.Data.Contexts;
using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            try
            {
                if (!_options.Value.StartParse)
                    return;
                    
                _parseLogger.LogWarning("Start parse");
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                //Посинаєм парсити
                var parser = ParserFactory.CreatePageParser(BaseLayer, _options.Value.BaseUrl, _parseLogger);
                var parseResults = (await parser.ParseAsync("/dictionary/english-ukrainian")).ToList();

                //Вибераємо ті слова де парс видав помилку через нагрузку
                var failedPages = parseResults.Where(x => !string.IsNullOrEmpty(x.Source)).ToList();
                _parseLogger.LogWarning("Number fail results : {Count}", failedPages.Count());
                
                var failedTask = (from failPage in failedPages
                    let wordParser = ParserFactory.CreatePageParser(WordLevel, _options.Value.BaseUrl, _parseLogger)
                    select wordParser.ParseAsync(failPage.Source!)).ToList();

                //Парсимо повторно і докідаємо результат до решти
                var secondTryResults = await Task.WhenAll(failedTask);
                foreach (var result in secondTryResults)
                    parseResults.AddRange(result);
                    
                //Приводимо до моделі в бд і відкидаємо провальні результати парсингу
                var dictionary = parseResults.Where(x => !string.IsNullOrEmpty(x.EnWord)).Select(x => new Word
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
                var dictionaryRepository = scope.ServiceProvider.GetRequiredService<IParserRepository>();
                
                //вибераємо на добавлення в базу ті слова яких нема в базі
                var uniqueWords = await dictionaryRepository.GetUniqueRecords(dictionary, db.Database.GetDbConnection(), db.Database.CurrentTransaction?.GetDbTransaction());
                dictionary = dictionary.Where(x => uniqueWords.Contains(x.Value)).ToList();
                
                if (dictionary.Count > 0)
                {
                    await dictionaryRepository.AddRangeAsync(db, dictionary);

                    await dictionaryRepository.AddRelations(JsonSerializer.Serialize(parseResults
                        .Where(x => x.RelatedWords is not null && x.RelatedWords.Any()).Select(x => new
                        {
                            x.EnWord,
                            x.RelatedWords
                        }).ToList()), db.Database.GetDbConnection(), db.Database.CurrentTransaction?.GetDbTransaction());

                    await transaction.CommitAsync(stoppingToken);
                }
                
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                _parseLogger.LogWarning(
                    "Elapsed milliseconds: {ElapsedMilliseconds} ms, number added words {DictionaryCount}",
                    elapsedMilliseconds, dictionary.Count);
            }
            catch (Exception e)
            {
                _parseLogger.LogError(e, "Error ocured ExecuteAsync in ParseService");
            }
        }
    }
}