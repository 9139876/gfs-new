using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class AssetEntity : IGuidKeyEntity
{
    public Guid Id { get; set; }
    public Guid MarketId { get; set; }
    public string Name { get; set; }


    #region Navigation

    public MarketEntity? Market { get; set; }
    public List<QuotesProviderAssetEntity> QuotesProviderAssets { get; set; } = new();

    #endregion
    
}

public class AssetEntityConfiguration : IEntityTypeConfiguration<AssetEntity>
{
    public void Configure(EntityTypeBuilder<AssetEntity> builder)
    {
        builder.ToTable("Assets");
        builder.HasKey(e => e.Id);
        builder.HasOne<MarketEntity>()
            .WithMany()
            .HasForeignKey(e => e.MarketId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}