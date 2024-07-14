using GFS.Common.Attributes.Validation;
using GFS.EF.Entities;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class QuoteEntity : GuidKeyEntity
{
    public Guid AssetId { get; set; }
    
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }
    
    public TimeFrameEnum TimeFrame { get; set; }
    
    [UtcDate]
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

public class QuoteEntityComparerByTfAndDate : IEqualityComparer<QuoteEntity>
{
    public bool Equals(QuoteEntity? x, QuoteEntity? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.TimeFrame == y.TimeFrame && x.Date.Equals(y.Date);
    }

    public int GetHashCode(QuoteEntity obj)
    {
        return HashCode.Combine((int)obj.TimeFrame, obj.Date);
    }
}