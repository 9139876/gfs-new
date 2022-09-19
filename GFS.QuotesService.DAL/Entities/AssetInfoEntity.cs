using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.QuotesService.DAL.Entities;

public class AssetInfoEntity : IGuidKeyEntity
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }

    /// <summary> Валюта расчётов </summary>
    public string? Currency { get; set; }

    /// <summary> Шаг цены </summary>
    public decimal? MinPriceIncrement { get; set; }

    /// <summary> Лотность инструмента. Возможно совершение операций только на количества ценной бумаги, кратные параметру lot </summary>
    public int? Lot { get; set; }

    /// <summary> Дата IPO акции в часовом поясе UTC </summary>
    public DateTime? IpoDate { get; set; }

    /// <summary> Сектор экономики </summary>
    public string? Sector { get; set; }

    #region Navigation

    public AssetEntity? Asset { get; set; }

    #endregion
}

public class AssetInfoEntityConfiguration : IEntityTypeConfiguration<AssetInfoEntity>
{
    public void Configure(EntityTypeBuilder<AssetInfoEntity> builder)
    {
        builder.ToTable("AssetInfos");
        builder.HasKey(e => e.Id);
        builder.HasOne<AssetEntity>()
            .WithOne()
            .HasForeignKey<AssetInfoEntity>(e => e.AssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}