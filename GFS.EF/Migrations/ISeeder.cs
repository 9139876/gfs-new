namespace GFS.EF.Migrations
{
    public interface ISeeder : IDisposable
    {
        Task Seed();
    }
}
