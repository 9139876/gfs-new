using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GFS.EF.Entities;
using GFS.QuotesService.Api.Enum;

namespace GFS.QuotesService.DAL.Entities;

public class QuotesProviderAssetEntity : GuidKeyEntity
{
    public Guid AssetId { get; set; }
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }
    public string GetQuotesRequest { get; set; }

    #region Navigation

    public AssetEntity? Asset { get; set; }
    public List<QuoteEntity> Quotes { get; set; } = new();
    
    public List<BackgroundWorkerTaskEntity> BackgroundWorkerTasks { get; set; } = new();

    #endregion
}

public class QuotesProviderAssetEntityConfiguration : IEntityTypeConfiguration<QuotesProviderAssetEntity>
{
    public void Configure(EntityTypeBuilder<QuotesProviderAssetEntity> builder)
    {
        builder.ToTable("QuotesProviderAssets");
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Asset)
            .WithMany(e => e.QuotesProviderAssets)
            .HasForeignKey(e => e.AssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}