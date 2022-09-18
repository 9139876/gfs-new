using GFS.EF.Entities;
using GFS.QuotesService.Api.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

/// <summary> Торговая площадка + Тип инструмента </summary>
public class MarketEntity : IGuidKeyEntity
{
    public Guid Id { get; set; }
    public MarketTypeEnum MarketType { get; set; }
    public AssetTypeEnum AssetType { get; set; }

    #region Navigation
    
    public List<AssetEntity> Assets { get; set; } = new();
    
    #endregion
}

public class MarketEntityConfiguration : IEntityTypeConfiguration<MarketEntity>
{
    public void Configure(EntityTypeBuilder<MarketEntity> builder)
    {
        builder.ToTable("Markets");
        builder.HasKey(e => e.Id);
    }
}