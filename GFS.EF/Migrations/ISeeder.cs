using System;
using System.Threading.Tasks;

namespace GFS.EF.Migrations
{
    public interface ISeeder : IDisposable
    {
        Task Seed();
    }
}
