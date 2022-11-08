namespace GFS.EF.Entities
{
    public interface IEntity
    {
    }

    public interface IEntityWithKey<out T> : IEntity
        where T : IComparable
    {
        T Id { get; }
    }

    public abstract class GuidKeyEntity : IEntityWithKey<Guid>
    {
        protected GuidKeyEntity()
        {
            Id = Guid.NewGuid();            
        }
        
        public Guid Id { get; protected set; }
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