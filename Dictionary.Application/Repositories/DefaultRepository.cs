using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Repositories;

public class DefaultRepository
{
    private readonly ILogger<DefaultRepository> _logger;

    protected DefaultRepository(ILogger<DefaultRepository> logger)
    {
        _logger = logger;
    }
    
    public async Task AddRangeAsync<TEntity>(DbContext context, IEnumerable<TEntity> collection, IDbContextTransaction? transaction = null)
        where TEntity : class
    {
        try
        {
            await context.Set<TEntity>().AddRangeAsync(collection);
            await context.SaveChangesAsync();

            if (transaction is not null)
                await transaction.CommitAsync();

        }
        catch (Exception e)
        {
            _logger.LogError(e, "error ocured AddRange");
            if (transaction is not null)
                await transaction.RollbackAsync();
        }
    }
}