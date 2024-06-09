using GFS.Common.Attributes.Validation;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class UpdateQuotesTaskEntity : GuidKeyEntity
{
    public Guid AssetByProviderId { get; init; }

    [UtcDate]
    public DateTime LastQuoteDate { get; set; }

    public bool IsActive { get; set; }

    #region Navigation

    public AssetProviderCompatibilityEntity? AssetByProvider { get; set; }

    #endregion
}

public class UpdateQuotesTaskEntityConfiguration : IEntityTypeConfiguration<UpdateQuotesTaskEntity>
{
    public void Configure(EntityTypeBuilder<UpdateQuotesTaskEntity> builder)
    {
        builder.ToTable("UpdateQuotesTasks");
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.AssetByProvider)
            .WithMany(e => e.UpdateQuotesTasks)
            .HasForeignKey(e => e.AssetByProviderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.AssetByProviderId).IsUnique();
    }
}