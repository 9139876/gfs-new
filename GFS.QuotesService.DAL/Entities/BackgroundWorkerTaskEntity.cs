using GFS.EF.Entities;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.Api.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class BackgroundWorkerTaskEntity : GuidKeyEntity
{
    public Guid? AssetId { get; set; }
    
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }

    public GetQuotesTaskTypeEnum TaskTaskType { get; set; }
    
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
        builder.HasOne(e => e.Asset)
            .WithMany()
            .HasForeignKey(e => e.AssetId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(e => new { e.AssetId, e.QuotesProviderType, TaskType = e.TaskTaskType }).IsUnique();
    }
}