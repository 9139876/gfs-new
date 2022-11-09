using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GFS.EF.Entities;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.DAL.Models;

namespace GFS.QuotesService.DAL.Entities;

public class QuotesProviderAssetEntity : GuidKeyEntity
{
    public Guid AssetId { get; set; }
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }
    public string GetQuotesRequest { get; set; }

    #region Navigation

    public AssetEntity? Asset { get; set; }

    #endregion
}

public class QuotesProviderAssetEntityConfiguration : IEntityTypeConfiguration<QuotesProviderAssetEntity>
{
    public void Configure(EntityTypeBuilder<QuotesProviderAssetEntity> builder)
    {
        builder.ToTable("QuotesProviderAssets");
        builder.HasKey(e => e.Id);
        builder.HasOne<AssetEntity>()
            .WithMany()
            .HasForeignKey(e => e.AssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}