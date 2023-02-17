using GFS.EF.Entities;
using GFS.QuotesService.Api.Common.Enum;
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

    public bool IsCompatibility { get; set; }

    #region Navigation

    public AssetEntity? Asset { get; set; }

    #endregion
}

public class AssetProviderCompatibilityEntityConfiguration : IEntityTypeConfiguration<AssetProviderCompatibilityEntity>
{
    public void Configure(EntityTypeBuilder<AssetProviderCompatibilityEntity> builder)
    {
        builder.ToTable("AssetProviderCompatibilities");
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Asset)
            .WithMany()
            .HasForeignKey(e => e.AssetId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(e => new { e.AssetId, e.QuotesProviderType }).IsUnique();
    }
}