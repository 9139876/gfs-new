﻿using System.Linq.Expressions;
using EFCore.BulkExtensions;
using GFS.EF.Context;
using GFS.EF.Entities;
using GFS.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GFS.EF.Repository;

public interface IRepository<TEntity>
    where TEntity : GuidKeyEntity
{
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = null);
    IQueryable<TEntity> GetBySelectors(IEnumerable<Func<IQueryable<TEntity>, IQueryable<TEntity>>> selectors);
    Task<TEntity> SingleOrFailByIdAsync(Guid id);
    Task<TEntity?> TryGetById(Guid id);
    Task<bool> Exist(Expression<Func<TEntity, bool>>? predicate = null);
    Task<bool> Exist(Guid id);
    TEntity Insert(TEntity entity);
    void InsertRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    Task DeleteById(Guid id);
    void DeleteRange(IEnumerable<TEntity> entities);
    Task DeleteRangeByIds(IEnumerable<Guid> ids);

    Task BulkInsertRangeAsync(IList<TEntity> entities);
    Task BulkUpdateAsync(IList<TEntity> entities);
    Task BulkDeleteAsync(IList<TEntity> entities);
    // Task BulkReadAsync(IList<TEntity> entities);
    
    /// <summary>
    /// Upsert
    /// </summary>
    Task BulkInsertOrUpdateAsync(IList<TEntity> entities);
    
    /// <summary>
    /// Sync
    /// </summary>
    Task BulkInsertOrUpdateOrDeleteAsync(IList<TEntity> entities);
}

public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : GuidKeyEntity
{
    private readonly GfsDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(GfsDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = null)
        => predicate != null
            ? _dbSet.Where(predicate)
            : _dbSet;

    public IQueryable<TEntity> GetBySelectors(IEnumerable<Func<IQueryable<TEntity>, IQueryable<TEntity>>> selectors)
        => selectors.Aggregate(_dbSet.AsQueryable(), (total, next) => next(total));

    public Task<TEntity> SingleOrFailByIdAsync(Guid id)
        => _dbSet.Where(e => e.Id == id).SingleOrFailAsync();

    public Task<TEntity?> TryGetById(Guid id)
        => _dbSet.Where(e => e.Id == id).SingleOrDefaultAsync();

    public Task<bool> Exist(Expression<Func<TEntity, bool>>? predicate = null)
        => predicate != null
            ? _dbSet.Where(predicate).AnyAsync()
            : _dbSet.AnyAsync();

    public Task<bool> Exist(Guid id)
        => _dbSet.Where(e => e.Id == id).AnyAsync();

    public TEntity Insert(TEntity entity)
        => _dbSet.Add(entity).Entity;

    public void InsertRange(IEnumerable<TEntity> entities)
        => _dbSet.AddRange(entities);

    public void Update(TEntity entity)
        => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities)
        => _dbSet.UpdateRange(entities);

    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);

    public async Task DeleteById(Guid id)
    {
        var entity = await TryGetById(id);

        if (entity != null)
            Delete(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
        => _dbSet.RemoveRange(entities);

    public async Task DeleteRangeByIds(IEnumerable<Guid> ids)
    {
        var entities = await Get(e => ids.Contains(e.Id)).ToListAsync();

        if (entities.Any())
            DeleteRange(entities);
    }

    #region Bulk

    public async Task BulkInsertRangeAsync(IList<TEntity> entities)
        => await _context.BulkInsertAsync(entities);

    public async Task BulkUpdateAsync(IList<TEntity> entities)
        => await _context.BulkUpdateAsync(entities);

    public async Task BulkDeleteAsync(IList<TEntity> entities)
        => await _context.BulkDeleteAsync(entities);

    public async Task BulkInsertOrUpdateAsync(IList<TEntity> entities)
        => await _context.BulkInsertOrUpdateAsync(entities);

    public async Task BulkInsertOrUpdateOrDeleteAsync(IList<TEntity> entities)
        => await _context.BulkInsertOrUpdateOrDeleteAsync(entities);

    #endregion
}