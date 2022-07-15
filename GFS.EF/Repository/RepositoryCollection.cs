using GFS.EF.Entities;

namespace GFS.EF.Repository
{
    public interface IRepositoryCollection
    {
        public IRepository<T> GetRepository<T>()
            where T : class, IGuidKeyEntity;
    }
}