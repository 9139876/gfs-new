using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        Task<TEntity> GetOrFailById(Guid id);
        Task<TEntity> SingleOrFailById(Guid id);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task SaveChanges();
    }

    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IGuidKeyEntity
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

        public Task<TEntity> GetOrFailById(Guid id)
            => _dbSet.Where(e => e.Id == id).GetOrFailAsync();

        public Task<TEntity> SingleOrFailById(Guid id)
            => _dbSet.Where(e => e.Id == id).SingleOrFailAsync();

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

        public void DeleteRange(IEnumerable<TEntity> entities)
            => _dbSet.RemoveRange(entities);

        public Task SaveChanges()
            => _context.SaveChangesAsync();
    }
}