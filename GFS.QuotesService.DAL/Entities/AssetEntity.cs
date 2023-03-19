using GFS.EF.Entities;
using GFS.QuotesService.Api.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#pragma warning disable CS8618

namespace GFS.QuotesService.DAL.Entities;

public class AssetEntity : GuidKeyEntity
{
    public MarketTypeEnum MarketType { get; init; }
    public AssetTypeEnum AssetType { get; init; }

    /// <summary> Торговая площадка </summary>
    public string Exchange { get; init; }

    /// <summary> Человекочитаемое имя </summary>
    public string Name { get; init; }

    /// <summary> Financial Instrument Global Identifier </summary>
    public string FIGI { get; init; }

    /// <summary> Короткий код инструмента </summary>
    public string Ticker { get; init; }

    /// <summary> Класс-код инструмента </summary>
    public string ClassCode { get; init; }

    /// <summary> International Securities Identification Number - Международный идентификационный код ценной бумаги </summary>
    public string? ISIN { get; init; }

    #region Equals

    public override bool Equals(object? obj)
        => obj is AssetEntity other && other.FIGI == FIGI;

    public override int GetHashCode()
        => FIGI.GetHashCode();

    #endregion

    #region Navigation

    public AssetInfoEntity? AssetInfo { get; set; }

    public List<QuoteEntity> Quotes { get; set; } = new();

    public List<AssetProviderCompatibilityEntity> ProviderCompatibilities { get; set; } = new();

    #endregion
}

public class AssetEntityConfiguration : IEntityTypeConfiguration<AssetEntity>
{
    public void Configure(EntityTypeBuilder<AssetEntity> builder)
    {
        builder.ToTable("Assets");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.FIGI).IsUnique();
    }
}