using System;
using System.Threading.Tasks;

namespace GFS.EF.Seed
{
    public interface ISeeder : IDisposable
    {
        Task Seed();
    }
}