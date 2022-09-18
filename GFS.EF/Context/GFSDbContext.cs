using System.Reflection;
using GFS.EF.Entities;
using GFS.EF.Repository;
using Microsoft.EntityFrameworkCore;

namespace GFS.EF.Context
{
    public abstract class GfsDbContext : DbContext, IDbContext
    {
        protected GfsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetEntitiesAssembly());
        }

        protected abstract Assembly GetEntitiesAssembly();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker.DetectChanges();

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

        public IRepository<T> GetRepository<T>()
            where T : class, IGuidKeyEntity
            => new GenericRepository<T>(this);
    }
}