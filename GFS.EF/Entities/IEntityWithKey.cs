namespace GFS.EF.Entities
{
    public interface IEntityWithKey<T> where T : IComparable
    {
        T Id { get; set; }
    }
}
