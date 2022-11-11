using GFS.EF.Entities;
using GFS.GrailCommon.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class QuoteEntity : GuidKeyEntity
{
    public Guid QuotesProviderAssetId { get; set; }
    public TimeFrameEnum TimeFrame { get; set; }
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal Hi { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal? Volume { get; set; }

    #region Navigation

    public QuotesProviderAssetEntity? QuotesProviderAsset { get; set; }

    #endregion
}

public class QuoteEntityConfiguration : IEntityTypeConfiguration<QuoteEntity>
{
    public void Configure(EntityTypeBuilder<QuoteEntity> builder)
    {
        builder.ToTable("Quotes");
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.QuotesProviderAsset)
            .WithMany(e => e.Quotes)
            .HasForeignKey(e => e.QuotesProviderAssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(e => new { e.QuotesProviderAssetId, e.TimeFrame });
    }
}