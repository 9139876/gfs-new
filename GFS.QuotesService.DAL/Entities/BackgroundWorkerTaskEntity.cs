using GFS.EF.Entities;
using GFS.QuotesService.Api.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class BackgroundWorkerTaskEntity : GuidKeyEntity
{
    public Guid AssetId { get; set; }
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }
    
    #region Navigation
    
    public AssetEntity? Asset { get; set; }
    
    #endregion
}

public class BackgroundWorkerTaskEntityConfiguration : IEntityTypeConfiguration<BackgroundWorkerTaskEntity>
{
    public void Configure(EntityTypeBuilder<BackgroundWorkerTaskEntity> builder)
    {
        builder.ToTable("BackgroundWorkerTasks");
        builder.HasKey(e => e.Id);
        builder.HasOne<AssetEntity>()
            .WithMany()
            .HasForeignKey(e => e.AssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}