using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GFS.EF.Entities;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.DAL.Models;

namespace GFS.QuotesService.DAL.Entities;

public class QuotesProviderAssetEntity : GuidKeyEntity
{
    public Guid AssetId { get; set; }
    public QuotesProviderTypeEnum QuotesProviderType { get; set; }

    #region Navigation

    public AssetEntity? Asset { get; set; }

    #endregion
}

public class TinkoffAssetEntity : QuotesProviderAssetEntity
{
    [Column(TypeName = "jsonb")] public TinkoffGetQuotesRequestModel TinkoffGetQuotesRequest { get; set; }
}

public class FinamAssetEntity : QuotesProviderAssetEntity
{
    [Column(TypeName = "jsonb")] public FinamGetQuotesRequestModel FinamGetQuotesRequest { get; set; }
}

public class BcsExpressAssetEntity : QuotesProviderAssetEntity
{
    [Column(TypeName = "jsonb")] public BcsExpressGetQuotesRequestModel BcsExpressGetQuotesRequest { get; set; }
}

public class InvestingComAssetEntity : QuotesProviderAssetEntity
{
    [Column(TypeName = "jsonb")] public InvestingComGetQuotesRequestModel InvestingComGetQuotesRequest { get; set; }
}

public class QuotesProviderAssetEntityConfiguration : IEntityTypeConfiguration<QuotesProviderAssetEntity>
{
    public void Configure(EntityTypeBuilder<QuotesProviderAssetEntity> builder)
    {
        builder.ToTable("QuotesProviderAssets")
            .HasDiscriminator(c => c.QuotesProviderType)
            .HasValue<TinkoffAssetEntity>(QuotesProviderTypeEnum.Tinkoff)
            .HasValue<FinamAssetEntity>(QuotesProviderTypeEnum.Finam)
            .HasValue<BcsExpressAssetEntity>(QuotesProviderTypeEnum.BcsExpress)
            .HasValue<InvestingComAssetEntity>(QuotesProviderTypeEnum.InvestingCom);
        builder.HasKey(e => e.Id);
        builder.HasOne<AssetEntity>()
            .WithMany()
            .HasForeignKey(e => e.AssetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}