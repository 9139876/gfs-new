namespace GFS.EF.Entities
{
    public interface IEntity
    {
    }

    public interface IEntityWithKey<T> : IEntity
        where T : IComparable
    {
        T Id { get; set; }
    }

    public interface IGuidKeyEntity : IEntityWithKey<Guid>
    {
    }

    public interface IUpdateTrackingEntity : IEntity
    {
        public DateTime? UpdatedAt { get; set; }
    }

    public interface ICreateTrackingEntity : IEntity
    {
        DateTime CreatedAt { get; set; }
    }
}