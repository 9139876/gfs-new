using GFS.EF.Entities;

namespace GFS.EF.Repository
{
    public interface IDbContext
    {
        IRepository<T> GetRepository<T>()
            where T : class, IGuidKeyEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}