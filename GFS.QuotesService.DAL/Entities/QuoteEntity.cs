using GFS.EF.Entities;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class QuoteEntity : GuidKeyEntity
{
    public Guid AssetId { get; set; }
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }
    public TimeFrameEnum TimeFrame { get; set; }
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal? Volume { get; set; }

    #region Navigation

    public AssetEntity? Asset { get; set; }

    #endregion
}

public class QuoteEntityConfiguration : IEntityTypeConfiguration<QuoteEntity>
{
    public void Configure(EntityTypeBuilder<QuoteEntity> builder)
    {
        builder.ToTable("Quotes");
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Asset)
            .WithMany(e => e.Quotes)
            .HasForeignKey(e => e.AssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(e => new { e.AssetId, e.QuotesProviderType, e.TimeFrame });
        builder.HasIndex(e => new { e.AssetId, e.QuotesProviderType, e.TimeFrame, e.Date }).IsUnique();
    }
}