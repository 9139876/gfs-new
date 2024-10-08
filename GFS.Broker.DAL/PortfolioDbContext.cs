using System.Reflection;
using GFS.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace GFS.Broker.DAL
{
    public class PortfolioDbContext : GfsDbContext
    {
        public PortfolioDbContext(DbContextOptions options)
            : base(options)
        {
        }
        
        protected override Assembly GetEntitiesAssembly() => this.GetType().Assembly;
    }
}