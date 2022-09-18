namespace GFS.EF.Seed
{
    public interface ISeeder : IDisposable
    {
        Task Seed();
    }
}