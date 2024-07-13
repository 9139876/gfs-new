using GFS.EF.Entities;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Common.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

/// <summary>
/// Признак, что инструмент поддерживается провайдером
/// </summary>
public class AssetProviderCompatibilityEntity : GuidKeyEntity
{
    public Guid AssetId { get; set; }

    public QuotesProviderTypeEnum QuotesProviderType { get; set; }
    
    public TimeFrameEnum TimeFrame { get; set; }

    #region Navigation

    public AssetEntity? Asset { get; set; }

    public List<UpdateQuotesTaskEntity> UpdateQuotesTasks { get; set; } = new();

    #endregion
}

public class AssetProviderCompatibilityEntityConfiguration : IEntityTypeConfiguration<AssetProviderCompatibilityEntity>
{
    public void Configure(EntityTypeBuilder<AssetProviderCompatibilityEntity> builder)
    {
        builder.ToTable("AssetProviderCompatibilities");
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Asset)
            .WithMany(e => e.ProviderCompatibilities)
            .HasForeignKey(e => e.AssetId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(e => new { e.AssetId, e.QuotesProviderType }).IsUnique();
    }
}