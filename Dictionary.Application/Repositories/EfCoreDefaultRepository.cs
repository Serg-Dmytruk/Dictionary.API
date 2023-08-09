using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Repositories;

public class EfCoreDefaultRepository : IEfCoreDefaultRepository
{
    private readonly ILogger<EfCoreDefaultRepository> _logger;

    protected EfCoreDefaultRepository(ILogger<EfCoreDefaultRepository> logger)
    {
        _logger = logger;
    }

    public async Task AddRangeAsync<TEntity>(DbContext context, IEnumerable<TEntity> collection) where TEntity : class
    {
        try
        {
            await context.Set<TEntity>().AddRangeAsync(collection);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error ocured DefaultRepository.AddRangeAsync");
        }
    }
}