using System.Linq.Expressions;
using FluentResults;
using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Models;
using Promomash.Domain.Common;
using Promomash.EntityFramework.Context;

namespace Promomash.EntityFramework.Repositories;

public class BaseRepository<T> : IAsyncRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<T?> GetById(Guid id, Expression<Func<T, object>>? includePropertiesExpression = null)
    {
        var query = _dbContext.Set<T>();
        if (includePropertiesExpression != null)
        {
            return await query.Include(includePropertiesExpression).FirstOrDefaultAsync(x => x.Id == id);
        }

        return await query.FindAsync(id);
    }

    public virtual async Task<bool> Exists(Expression<Func<T, bool>> expression,
        Expression<Func<T, object>>? includePropertiesExpression = null)
    {
        var query = _dbContext.Set<T>();
        if (includePropertiesExpression != null)
        {
            return await query.Include(includePropertiesExpression).AnyAsync(expression);
        }

        return await query.AnyAsync(expression);
    }

    public async Task<IReadOnlyList<T>> ListAll(Expression<Func<T, object>>? includePropertiesExpression = null)
    {
        var query = _dbContext.Set<T>();
        if (includePropertiesExpression != null)
        {
            return await query.Include(includePropertiesExpression).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> ListAllWithoutTracking(
        Expression<Func<T, object>>? includePropertiesExpression = null)
    {
        var query = _dbContext.Set<T>();
        if (includePropertiesExpression != null)
        {
            return await query.Include(includePropertiesExpression).AsNoTracking().ToListAsync();
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async virtual Task<PaginatedList<T>> GetPagedReponse(int page,
        int size,
        Expression<Func<T, object>>? includePropertiesExpression = null)
    {
        var queryable = _dbContext.Set<T>().AsQueryable();
        List<T>? items;
        int count;
        if (includePropertiesExpression != null)
        {
            items = await queryable.Include(includePropertiesExpression).Skip((page - 1) * size).Take(size)
                .AsNoTracking().ToListAsync();
            count = await queryable.CountAsync();
            return new PaginatedList<T>(items, count, page, size);
        }

        items = await queryable.Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        count = await queryable.CountAsync();
        return new PaginatedList<T>(items, count, page, size);
    }

    public async Task<Result<T>> Add(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}