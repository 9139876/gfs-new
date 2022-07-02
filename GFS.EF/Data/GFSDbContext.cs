using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GFS.Common.Exceptions;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.EF.Data
{
    public class GfsDbContext : DbContext
    {
        public async Task<TEntity?> TryGetOneAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntityWithKey<TKey>
            where TKey : IComparable
        {
            Expression<Func<TEntity, bool>> predicate = entity => entity.Id.Equals(id);

            return await TryGetOneAsync(predicate);
        }

        public async Task<TEntity?> TryGetOneAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return await Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        /// <exception cref="NotFoundException"></exception>
        public async Task<TEntity> GetOrFailAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntityWithKey<TKey>
            where TKey : IComparable
        {
            return await TryGetOneAsync<TEntity, TKey>(id) ?? throw new NotFoundException(typeof(TEntity));
        }

        /// <exception cref="NotFoundException"></exception>
        public async Task<TEntity> GetOrFailAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return await TryGetOneAsync(predicate) ?? throw new NotFoundException(typeof(TEntity));
        }

        public async Task<bool> ExistAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntityWithKey<TKey>
            where TKey : IComparable
        {
            Expression<Func<TEntity, bool>> predicate = entity => entity.Id.Equals(id);

            return await ExistAsync(predicate);
        }

        public async Task<bool> ExistAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return await Set<TEntity>().Where(predicate).AnyAsync();
        }

        public async Task<TEntity> GetSingleOrFailAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntityWithKey<TKey>
            where TKey : IComparable
        {
            Expression<Func<TEntity, bool>> predicate = entity => entity.Id.Equals(id);

            return await GetSingleOrFailAsync(predicate);
        }

        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="SingleException"></exception>
        public async Task<TEntity> GetSingleOrFailAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var result = await Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .Take(2)
                .ToListAsync();

            if (result.FirstOrDefault() == null)
                throw new NotFoundException(typeof(TEntity));
            else if (result.LastOrDefault() != null)
                throw new SingleException();

            return result.First();
        }

        public async Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return await Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<List<TEntity>> GetAsync<TEntity>(IQueryable<TEntity> query)
            where TEntity : class
        {
            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            BeforeSaveTriggers();

            // avoid calling DetectChanges again
            ChangeTracker.AutoDetectChangesEnabled = false;
            var result = await base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        private void BeforeSaveTriggers()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var createdEntityEntry in ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                if (createdEntityEntry.Entity is IGuidKeyEntity guidKeyEntity)
                    guidKeyEntity.Id = guidKeyEntity.Id == default ? Guid.NewGuid() : guidKeyEntity.Id;

                if (createdEntityEntry.Entity is ICreateTrackingEntity createTrackingEntity)
                    createTrackingEntity.CreatedAt = utcNow;

                if (createdEntityEntry.Entity is IUpdateTrackingEntity updateTrackingEntity)
                    updateTrackingEntity.UpdatedAt = null;
            }

            foreach (var updatedEntityEntry in ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                if (updatedEntityEntry.Entity is IUpdateTrackingEntity updateTrackingEntity)
                    updateTrackingEntity.UpdatedAt = utcNow;

                if (updatedEntityEntry.Entity is ICreateTrackingEntity)
                    updatedEntityEntry.Property(nameof(ICreateTrackingEntity.CreatedAt)).IsModified = false;
            }
        }
    }
}