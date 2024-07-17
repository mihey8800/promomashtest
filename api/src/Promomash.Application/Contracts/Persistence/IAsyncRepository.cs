using System.Linq.Expressions;
using Promomash.Application.Models;

namespace Promomash.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    Task<T?> GetById(Guid id, Expression<Func<T, object>>? includePropertiesExpression = null);

    Task<bool> Exists(Expression<Func<T, bool>> expression,
        Expression<Func<T, object>>? includePropertiesExpression = null);

    Task<IReadOnlyList<T>> ListAll(Expression<Func<T, object>>? includePropertiesExpression = null);

    Task<IReadOnlyList<T>> ListAllWithoutTracking(Expression<Func<T, object>>? includePropertiesExpression = null);

    Task<Result<T>> Add(T entity);

    Task Update(T entity);

    Task Delete(T entity);

    Task<PaginatedList<T>> GetPagedReponse(int page,
        int size,
        Expression<Func<T, object>>? includePropertiesExpression = null);
}