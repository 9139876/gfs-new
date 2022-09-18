using System.Reflection;
using GFS.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace GFS.QuotesService.DAL;

public class QuotesServiceDbContext : GfsDbContext
{
    public QuotesServiceDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override Assembly GetEntitiesAssembly() => this.GetType().Assembly;
}