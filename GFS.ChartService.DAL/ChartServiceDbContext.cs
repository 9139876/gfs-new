using System.Reflection;
using GFS.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace GFS.ChartService.DAL;

public class ChartServiceDbContext : GfsDbContext
{
    public ChartServiceDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override Assembly GetEntitiesAssembly() => this.GetType().Assembly;
}