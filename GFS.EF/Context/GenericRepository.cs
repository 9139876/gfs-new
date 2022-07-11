using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.EF.Context
{
    public interface IGenericRepository<T> where T : class, IGuidKeyEntity
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(Guid id);
        Task<bool> Add(T entity);
        Task<bool> Delete(Guid id);
        Task<bool> Upsert(T entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> 
        where T : class, IGuidKeyEntity
    {
        protected DbContext _context;
        internal DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> All()
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}