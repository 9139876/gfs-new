using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#pragma warning disable CS8618

namespace GFS.ChartService.DAL.Entities;

public class ProjectInfoEntity : GuidKeyEntity
{
    public string Name { get; init; }

    public Guid FileGuid { get; init; }

    public DateTime CreatedDate { get; init; }

    public DateTime ModificationDate { get; init; }

    public int ProjectVersion { get; init; }
}

public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectInfoEntity>
{
    public void Configure(EntityTypeBuilder<ProjectInfoEntity> builder)
    {
        builder.ToTable("ProjectInfos");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name).IsUnique();
    }
}