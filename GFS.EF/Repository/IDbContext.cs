using GFS.EF.Entities;

namespace GFS.EF.Repository
{
    public interface IDbContext
    {
        IRepository<T> GetRepository<T>()
            where T : GuidKeyEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BulkSaveChangesAsync();
    }
}