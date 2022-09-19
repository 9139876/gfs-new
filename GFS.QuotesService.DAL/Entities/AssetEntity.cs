using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class AssetEntity : IGuidKeyEntity
{
    public Guid Id { get; set; }

    [Obsolete($"Using {nameof(Exchange)}")]
    public Guid MarketId { get; set; }

    /// <summary> Торговая площадка </summary>
    public string Exchange { get; set; }

    /// <summary> Человекочитаемое имя </summary>
    public string Name { get; set; }

    /// <summary> Financial Instrument Global Identifier </summary>
    public string FIGI { get; set; }

    /// <summary> Короткий код инструмента </summary>
    public string Ticker { get; set; }

    /// <summary> Класс-код инструмента </summary>
    public string ClassCode { get; set; }

    /// <summary> International Securities Identification Number - Международный идентификационный код ценной бумаги </summary>
    public string ISIN { get; set; }

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