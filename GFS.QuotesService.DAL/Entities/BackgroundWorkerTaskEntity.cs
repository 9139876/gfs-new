using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class BackgroundWorkerTaskEntity : GuidKeyEntity
{
    public Guid QuotesProviderAssetId { get; set; }

    #region Navigation

    public QuotesProviderAssetEntity? QuotesProviderAsset { get; set; }

    #endregion
}

public class BackgroundWorkerTaskEntityConfiguration : IEntityTypeConfiguration<BackgroundWorkerTaskEntity>
{
    public void Configure(EntityTypeBuilder<BackgroundWorkerTaskEntity> builder)
    {
        builder.ToTable("BackgroundWorkerTasks");
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.QuotesProviderAsset)
            .WithMany(e => e.BackgroundWorkerTasks)
            .HasForeignKey(e => e.QuotesProviderAssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}