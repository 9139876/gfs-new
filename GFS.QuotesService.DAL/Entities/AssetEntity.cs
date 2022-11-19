using GFS.EF.Entities;
using GFS.QuotesService.Api.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class AssetEntity : GuidKeyEntity
{
    public MarketTypeEnum MarketType { get; set; }
    public AssetTypeEnum AssetType { get; set; }

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

    #region Equals

    public override bool Equals(object? obj)
        => obj is AssetEntity other && other.Ticker == Ticker;

    public override int GetHashCode()
        => Ticker.GetHashCode();

    #endregion

    #region Navigation

    public AssetInfoEntity? AssetInfo { get; set; }

    #endregion
}

public class AssetEntityConfiguration : IEntityTypeConfiguration<AssetEntity>
{
    public void Configure(EntityTypeBuilder<AssetEntity> builder)
    {
        builder.ToTable("Assets");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Ticker).IsUnique();
    }
}