using System.Linq.Expressions;
using GFS.EF.Context;
using GFS.EF.Entities;
using GFS.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GFS.EF.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class, IGuidKeyEntity
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = null);
        Task<TEntity> SingleOrFailById(Guid id);
        Task<TEntity?> TryGetById(Guid id);
        Task<bool> Exist(Expression<Func<TEntity, bool>>? predicate = null);
        Task<bool> Exist(Guid id);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        Task DeleteById(Guid id);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task DeleteRangeByIds(IEnumerable<Guid> ids);
    }

    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IGuidKeyEntity
    {
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GfsDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = null)
            => predicate != null
                ? _dbSet.Where(predicate)
                : _dbSet;

        public Task<TEntity> SingleOrFailById(Guid id)
            => _dbSet.Where(e => e.Id == id).SingleOrFailAsync();

        public Task<TEntity?> TryGetById(Guid id)
            => _dbSet.Where(e => e.Id == id).SingleOrDefaultAsync();

        public Task<bool> Exist(Expression<Func<TEntity, bool>>? predicate = null)
            => predicate != null
                ? _dbSet.Where(predicate).AnyAsync()
                : _dbSet.AnyAsync();

        public Task<bool> Exist(Guid id)
            => _dbSet.Where(e => e.Id == id).AnyAsync();

        public void Insert(TEntity entity)
            => _dbSet.Add(entity);

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
    }
}