using GFS.EF.Data;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.EF.Repository
{
    public class GenericRepository<T> where T : class, IGuidKeyEntity
    {
        protected GfsDbContext _context;

        private readonly DbSet<T> _dbSet;
        //public readonly ILogger _logger;

        public GenericRepository(
                GfsDbContext context)
            //ILogger logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            //_logger = logger;
        }

        //public virtual async Task<T> GetById(Guid id)
        //{
        //    return await _dbSet.FindAsync(id);
        //}

        //public virtual async Task<bool> Add(T entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //    return true;
        //}

        //public virtual Task<bool> Delete(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual Task<IEnumerable<T>> All()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        //{
        //    return await _dbSet.Where(predicate).ToListAsync();
        //}

        //public virtual Task<bool> Upsert(T entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}