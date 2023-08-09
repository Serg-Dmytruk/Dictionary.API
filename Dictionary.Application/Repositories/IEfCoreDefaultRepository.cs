using Microsoft.EntityFrameworkCore;

namespace Dictionary.Application.Repositories;

public interface IEfCoreDefaultRepository
{
    Task AddRangeAsync<TEntity>(DbContext context, IEnumerable<TEntity> collection) where TEntity : class;
}