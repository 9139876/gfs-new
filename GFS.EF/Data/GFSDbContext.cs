using System.Globalization;
using System.Linq.Expressions;
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
            return await Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetOrFailAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntityWithKey<TKey>
            where TKey : IComparable
        {
            return await TryGetOneAsync<TEntity,TKey>(id) ?? throw new NotFoundException(typeof(TEntity));
        }

        public async Task<TEntity?> GetOrFailAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return await TryGetOneAsync<TEntity>(predicate) ?? throw new NotFoundException(typeof(TEntity));
        }

        //ToDo: GetSingleOrFailAsync
        //ToDo: ExistAsync


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

                if(createdEntityEntry.Entity is IUpdateTrackingEntity updateTrackingEntity)
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
